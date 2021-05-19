using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SAP1EMU.Assembler;
using SAP1EMU.Engine;
using SAP1EMU.GUI.Contexts;
using SAP1EMU.Data.Lib;
using SAP1EMU.Lib;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace SAP1EMU.GUI.Controllers
{
    [Route("api/emulator/sap1")]
    [ApiController]
    public class SAP1EmulatorController : ControllerBase
    {
        #region Private Fields & Object Definitions 
        private IDecoder _decoder { get; set; }
        private Sap1EmuContext _sap1EmuContext { get; set; }
        private int _emulatorId { get; set; }
        private Dictionary<string, int> _instructionSets { get; set; }

        public class EmulatorPacket
        {
            [Required]
            public List<string> CodeList { get; set; }

            [Required]
            public string SetName { get; set; }
        }
        #endregion

        public SAP1EmulatorController(IDecoder decoder, Sap1EmuContext sap1EmuContext)
        {
            _decoder = decoder;
            _sap1EmuContext = sap1EmuContext;

            _emulatorId = _sap1EmuContext.Emulators.Single(Emulator => Emulator.Name == "SAP1").Id;
            _instructionSets = _sap1EmuContext.InstructionSets
                .Where(InstructionSet => InstructionSet.EmulatorId == _emulatorId)
                .ToDictionary(x => x.Name, x => x.Id);

        }


        /// <summary>
        /// Runs SAP1Emu compatable code and returns the emulated frames
        /// </summary>
        /// <param name="emulatorPacket"></param>
        /// <returns>A list of emulation frames</returns>
        /// <response code="200">Returns a list of emulation frames</response>
        /// <response code="400">Code contained syntax errors or sets does not exist</response> 
        /// <response code="500">Server Error. Contact Network Admin</response> 
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /emulate
        ///     {
        ///         "CodeList": [
        ///             "LDA 0xF",
        ///             "STA 0xE",
        ///             "OUT 0x0",
        ///             "HLT 0x0",
        ///             "...",
        ///             "0xA 0xA"
        ///         ],
        ///         "SetName": "SAP1Emu"
        ///     }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("emulate")]
        public ActionResult Post([FromBody] EmulatorPacket emulatorPacket)
        {
            EntityEntry<EmulationSessionMap> session;
            try
            {
                session = _sap1EmuContext.Add<EmulationSessionMap>(new EmulationSessionMap
                {
                    EmulationID = Guid.NewGuid(),
                    ConnectionID = null,
                    SessionStart = DateTime.UtcNow,
                    EmulatorId = _emulatorId,
                    StatusId = StatusFactory.GetStatus(StatusType.Pending).Id,
                    InstructionSetId = _instructionSets[emulatorPacket.SetName]
                });
                _sap1EmuContext.CodeSubmissions.Add(new CodeSubmission()
                {
                    EmulationID = session.Entity.EmulationID,
                    Code = emulatorPacket.CodeList,
                });
                _sap1EmuContext.SaveChanges();
            }
            catch(KeyNotFoundException)
            {
                return BadRequest(
                    // TODO: Set Type to supported_sets url
                    new ProblemDetails()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Type = "",
                        Title = "Invalid SetName...",
                        Detail = $"The Instruction Set `{emulatorPacket.SetName}` does not exist.",
                        Instance = HttpContext.Request.Path
                    }
                );
            }


            try
            {
                List<string> compiled_binary = Assemble.Parse(emulatorPacket.CodeList, emulatorPacket.SetName);
                RAMProgram rmp = new RAMProgram(compiled_binary);

                EngineProc engine = new EngineProc();
                engine.Init(rmp, _decoder, emulatorPacket.SetName);
                engine.Run();

                session.Entity.SessionEnd = DateTime.UtcNow;
                session.Entity.StatusId = StatusFactory.GetStatus(StatusType.Ok).Id;

                _sap1EmuContext.SaveChanges();
                return Ok(engine.FrameStack());
            }
            catch(ParseException pe)
            {
                session.Entity.SessionEnd = DateTime.UtcNow;
                session.Entity.StatusId = StatusFactory.GetStatus(StatusType.ParsingError).Id;


                if (pe.InnerException != null)
                {
                    string msg = (pe.Message + " " + pe.InnerException.Message);
                    _sap1EmuContext.ErrorLog.Add(new ErrorLog()
                    {
                        EmulationID = session.Entity.EmulationID,
                        ErrorMsg = msg
                    });

                    _sap1EmuContext.SaveChanges();
                    return BadRequest(msg);
                }
                else
                {
                    _sap1EmuContext.ErrorLog.Add(new ErrorLog()
                    {
                        EmulationID = session.Entity.EmulationID,
                        ErrorMsg = pe.Message
                    });

                    _sap1EmuContext.SaveChanges();
                    return BadRequest(pe.Message);
                }
            }
            catch(EngineRuntimeException ere)
            {
                session.Entity.SessionEnd = DateTime.UtcNow;
                session.Entity.StatusId = StatusFactory.GetStatus(StatusType.EmulationError).Id;

                if (ere.InnerException != null)
                {
                    string msg = (ere.Message + " " + ere.InnerException.Message);
                    _sap1EmuContext.ErrorLog.Add(new ErrorLog()
                    {
                        EmulationID = session.Entity.EmulationID,
                        ErrorMsg = msg
                    });

                    _sap1EmuContext.SaveChanges();
                    return BadRequest(ere.Message + " " + ere.InnerException.Message);
                }
                else
                {
                    _sap1EmuContext.ErrorLog.Add(new ErrorLog()
                    {
                        EmulationID = session.Entity.EmulationID,
                        ErrorMsg = ere.Message
                    });

                    _sap1EmuContext.SaveChanges();
                    return BadRequest(ere.Message);
                }
            }
            catch (Exception e)
            {
                session.Entity.SessionEnd = DateTime.UtcNow;
                session.Entity.StatusId = StatusFactory.GetStatus(StatusType.SystemError).Id;

                _sap1EmuContext.ErrorLog.Add(new ErrorLog()
                {
                    EmulationID = session.Entity.EmulationID,
                    ErrorMsg = e.Message
                });

                _sap1EmuContext.SaveChanges();
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}