using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using SAP1EMU.GUI.Contexts;
using SAP1EMU.GUI.Hubs;
using SAP1EMU.Data.Lib;
using SAP1EMU.SAP2.Assembler;
using SAP1EMU.SAP2.Lib;
using System;
using System.Linq;
using System.Collections.Generic;
using SAP1EMU.SAP2.Engine;

namespace SAP1EMU.GUI.Controllers
{
    [Route("api/emulator/sap2")]
    [ApiController]
    public class SAP2EmulatorController : Controller
    {
        #region Private Fields & Object Definitions 
        private Sap1EmuContext _sap1EmuContext { get; set; }
        private int _emulatorId { get; set; }
        private Dictionary<string, int> _instructionSets { get; set; }


        private readonly IHubContext<EmulatorHub> _hubContext;
        private readonly IDecoder _decoder;
        #endregion

        public SAP2EmulatorController(Sap1EmuContext sap1EmuContext, IHubContext<EmulatorHub> hubContext, IDecoder decoder)
        {
            _sap1EmuContext = sap1EmuContext;
            _hubContext = hubContext;
            _decoder = decoder;

           // _emulatorId = _sap1EmuContext.Emulators.AsNoTracking().Single(Emulator => Emulator.Name == "SAP2").Id;
            //_instructionSets = _sap1EmuContext.InstructionSets
            //    .Where(InstructionSet => InstructionSet.EmulatorId == _emulatorId)
            //    .AsNoTracking()
            //    .ToDictionary(x => x.Name, x => x.Id);
        }


        [HttpGet("session/create")]
        public IActionResult GetEmulationID()
        {
            Guid newSessionId = Guid.NewGuid();
            _sap1EmuContext.Add<EmulationSessionMap>(new EmulationSessionMap()
            {
                EmulationID = newSessionId,
                ConnectionID = null,
                SessionStart = DateTime.UtcNow,
                StatusId = (int)StatusType.Pending,
                EmulatorId = 2,
                InstructionSetId = 4
            });
            _sap1EmuContext.SaveChanges();

            return Ok(newSessionId);
        }

        
        [HttpPost("emulate")]
        public IActionResult StartEmulation([FromBody] SAP2CodePacket sap2CodePacket)
        {
            EmulationSessionMap session = null;
            try
            {
                session = _sap1EmuContext.EmulationSessionMaps
                    .Single(esm => esm.EmulationID == sap2CodePacket.EmulationID);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(sap2CodePacket.EmulationID);
            }
            

            if(session.StatusId != (int)StatusType.Pending)
            {
                string message = string.Empty;
                switch ((StatusType)session.StatusId)
                {
                    case StatusType.Ok:
                        message = "Emulation Complete: Please use 'GET: /session/{id}/recall' instead";
                        break;
                    case StatusType.SQLError:
                    case StatusType.ParsingError:
                    case StatusType.EmulationError:
                    case StatusType.SystemError:
                        message = "Emulation Errored: Please create new session";
                        break;
                    case StatusType.InProgress:
                        message = "Emulation In Progress: Please use 'POST: /{id}/resume' instead";
                        break;
                    default:
                        message = "Unknown Host Error";
                        break;
                }

                return BadRequest(
                    new
                    {
                        EmulationID = sap2CodePacket.EmulationID,
                        Status = StatusFactory.GetStatus((StatusType)session.StatusId),
                        Message = message
                    }
                );
            }


            // This will save the plain code
            try
            {
                _sap1EmuContext.Add<SAP2CodePacket>(sap2CodePacket);
                _sap1EmuContext.SaveChanges();
            }
            catch (Exception e)
            {
                _sap1EmuContext.ErrorLog.Add(
                    new ErrorLog
                    {
                        EmulationID = session.EmulationID,
                        ErrorMsg = "NON-FATAL SQL ERROR:\t" + e.Message + (e.InnerException != null ? "\t" + e.InnerException.Message : "")
                    }
                );
                session.SessionEnd = DateTime.UtcNow;
                session.StatusId = (int)StatusType.SystemError;

                _sap1EmuContext.SaveChanges();
            }


            SAP2BinaryPacket sap2BinaryPacket;

            // Assemble 
            try
            {
                sap2BinaryPacket = new SAP2BinaryPacket()
                {
                    EmulationID = sap2CodePacket.EmulationID,
                    Code = Assemble.Parse((List<string>)sap2CodePacket.Code),
                    SetName = sap2CodePacket.SetName
                };
            }
            catch(ParseException pe)
            {
                session.StatusId = (int)StatusType.ParsingError;
                session.SessionEnd = DateTime.UtcNow;

                string errorMsg = pe.Message + (pe.InnerException != null ? "\n" + pe.InnerException.Message : "");
                _sap1EmuContext.ErrorLog.Add(
                    new ErrorLog
                    {
                        EmulationID = session.EmulationID,
                        ErrorMsg = errorMsg
                    }
                );

                _sap1EmuContext.SaveChanges();
                return BadRequest(
                    new
                    {
                        EmulationID = sap2CodePacket.EmulationID,
                        Status = StatusFactory.GetStatus((StatusType)session.StatusId),
                        Message = errorMsg
                    }
                );
            }


            // Save Binary
            try
            {
                _sap1EmuContext.Add(sap2BinaryPacket);
                _sap1EmuContext.SaveChangesAsync(); // Might have to switch to sync
            }
            catch (Exception e)
            {
                _sap1EmuContext.ErrorLog.Add(
                    new ErrorLog
                    {
                        EmulationID = session.EmulationID,
                        ErrorMsg = "NON-FATAL SQL ERROR:\t" + e.Message + (e.InnerException != null ? "\t" + e.InnerException.Message : "")
                    }
                );

                session.SessionEnd = DateTime.UtcNow;
                session.StatusId = (int)StatusType.SystemError;

                _sap1EmuContext.SaveChanges();
            }


            // Run Emulator
            try
            {
                RAMProgram rmp = new RAMProgram((List<string>)sap2BinaryPacket.Code);

                EngineProc engine = new EngineProc();
                engine.Init(rmp, _decoder, sap2BinaryPacket.SetName);
                engine.Run();

                session.StatusId = (int)StatusType.Ok;
                session.SessionEnd = DateTime.UtcNow;
                _sap1EmuContext.SaveChanges();

                return Ok(engine.FrameStack());
            }
            catch (EngineRuntimeException ere)
            {
                session.StatusId = (int)StatusType.EmulationError;
                session.SessionEnd = DateTime.UtcNow;

                string errorMsg = ere.Message + (ere.InnerException != null ? "\n" + ere.InnerException.Message : "");
                _sap1EmuContext.ErrorLog.Add(
                    new ErrorLog
                    {
                        EmulationID = session.EmulationID,
                        ErrorMsg = errorMsg
                    }
                );
                _sap1EmuContext.SaveChanges();
                return BadRequest(
                    new
                    {
                        EmulationID = sap2CodePacket.EmulationID,
                        Status = StatusFactory.GetStatus((StatusType)session.StatusId),
                        Message = errorMsg
                    }
                );
            }
        }


        // TODO: finish this api call
        [HttpPost("{id}/resume")]
        public IActionResult ResumeEmulation(Guid id, [FromBody] string input)
        {
            return Ok(id);
        }

        // TODO: finish this api call
        [HttpGet("session/{id}/recall")]
        public IActionResult RecallSessionHistory(Guid id)
        {
            return Ok(id);
        }
    }
}
