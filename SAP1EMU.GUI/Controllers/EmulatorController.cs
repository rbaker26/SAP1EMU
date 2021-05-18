using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SAP1EMU.Assembler;
using SAP1EMU.Engine;
using SAP1EMU.GUI.Contexts;
using SAP1EMU.GUI.Models;
using SAP1EMU.Lib;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmulatorController : ControllerBase
    {
        private IDecoder _decoder { get; set; }
        private Sap1EmuContext _sap1EmuContext { get; set; }

        public EmulatorController(IDecoder decoder, Sap1EmuContext sap1EmuContext)
        {
            _decoder = decoder;
            _sap1EmuContext = sap1EmuContext;
        }

        public class EmulatorPacket
        {
            [Required]
            public List<string> CodeList { get; set; }

            [Required]
            public string SetName { get; set; }
        }


        /// <summary>
        /// Runs SAP1Emu compatable code and returns the emulated frames
        /// </summary>
        /// <param name="emulatorPacket"></param>
        /// <returns>A list of emulation frames</returns>
        /// <response code="200">Returns a list of emulation frames</response>
        /// <response code="400">Code contained syntax errors or sets does not exist</response> 
        /// <response code="500">Server Error. Contact Network Admin</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] EmulatorPacket emulatorPacket)
        {
            // TODO: Dispatch to seperate thread
            var temp = _sap1EmuContext.Add<CodeSubmit>(new CodeSubmit
            {
                EmulationId = Guid.NewGuid(),
                code = emulatorPacket.CodeList.Aggregate("", (current, s) => current + (s + ",")),
                submitted_at = DateTime.Now,
                Status = EmulationStatus.Pending
            });
            await _sap1EmuContext.SaveChangesAsync();
            try
            {

                List<string> compiled_binary = Assemble.Parse(emulatorPacket.CodeList, emulatorPacket.SetName);
                RAMProgram rmp = new RAMProgram(compiled_binary);

                EngineProc engine = new EngineProc();
                engine.Init(rmp, _decoder, emulatorPacket.SetName);
                engine.Run();

                _sap1EmuContext.CodeStore.First
                    (
                        code => code.EmulationId == temp.Entity.EmulationId
                    ).Status = EmulationStatus.Success;    
                await _sap1EmuContext.SaveChangesAsync();


                return Ok(engine.FrameStack());
            }
            catch(ParseException pe)
            {
                _sap1EmuContext.CodeStore.First
                   (
                       code => code.EmulationId == temp.Entity.EmulationId
                   ).Status = EmulationStatus.ParseError;

                if (pe.InnerException != null)
                {
                    _sap1EmuContext.SAP1ErrorLog.Add(new SAP1ErrorLog()
                    {
                        EmulationID = temp.Entity.EmulationId,
                        Error = (pe.Message + " " + pe.InnerException.Message)
                    });
                    await _sap1EmuContext.SaveChangesAsync();
                    return BadRequest(pe.Message + " " + pe.InnerException.Message);
                }
                else
                {
                    _sap1EmuContext.SAP1ErrorLog.Add(new SAP1ErrorLog()
                    {
                        EmulationID = temp.Entity.EmulationId,
                        Error = pe.Message
                    });
                    await _sap1EmuContext.SaveChangesAsync();
                    return BadRequest(pe.Message);
                }
            }
            catch(EngineRuntimeException ere)
            {
                _sap1EmuContext.CodeStore.First
                   (
                       code => code.EmulationId == temp.Entity.EmulationId
                   ).Status = EmulationStatus.EngineError;
                await _sap1EmuContext.SaveChangesAsync();

                if (ere.InnerException != null)
                {
                    _sap1EmuContext.SAP1ErrorLog.Add(new SAP1ErrorLog()
                    {
                        EmulationID = temp.Entity.EmulationId,
                        Error = (ere.Message + " " + ere.InnerException.Message)
                    });
                    await _sap1EmuContext.SaveChangesAsync();
                    return BadRequest(ere.Message + " " + ere.InnerException.Message);
                }
                else
                {
                    _sap1EmuContext.SAP1ErrorLog.Add(new SAP1ErrorLog()
                    {
                        EmulationID = temp.Entity.EmulationId,
                        Error = (ere.Message)
                    });
                    await _sap1EmuContext.SaveChangesAsync();
                    return BadRequest(ere.Message);
                }
            }
            catch (Exception e)
            {
                _sap1EmuContext.CodeStore.First
                   (
                       code => code.EmulationId == temp.Entity.EmulationId
                   ).Status = EmulationStatus.UnknownError;
                _sap1EmuContext.SAP1ErrorLog.Add(new SAP1ErrorLog()
                {
                    EmulationID = temp.Entity.EmulationId,
                    Error = "Unknown Error"
                });
                await _sap1EmuContext.SaveChangesAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}