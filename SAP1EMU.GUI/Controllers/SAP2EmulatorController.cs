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
        private Sap1EmuContext _sap1EmuContext { get; set; }
        private int _emulatorId { get; set; }
        private Dictionary<string, int> _instructionSets { get; set; }


        private readonly IHubContext<EmulatorHub> _hubContext;
        private readonly IDecoder _decoder;

        public SAP2EmulatorController(Sap1EmuContext sap1EmuContext, IHubContext<EmulatorHub> hubContext, IDecoder decoder)
        {
            _sap1EmuContext = sap1EmuContext;
            _hubContext = hubContext;
            _decoder = decoder;

            _emulatorId = _sap1EmuContext.Emulators.Single(Emulator => Emulator.Name == "SAP2").Id;
            _instructionSets = _sap1EmuContext.InstructionSets
                .Where(InstructionSet => InstructionSet.EmulatorId == _emulatorId)
                .ToDictionary(x => x.Name, x => x.Id);
        }


        [HttpGet("id")]
        public IActionResult GetEmulationID()
        {
            Guid newSessionId = Guid.NewGuid();
            _sap1EmuContext.Add<EmulationSessionMap>(new EmulationSessionMap()
            {
                EmulationID = newSessionId,
                ConnectionID = null,
                SessionStart = DateTime.UtcNow,
            });
            _sap1EmuContext.SaveChangesAsync(); // Might have to switch to sync

            return Ok(newSessionId);
        }

        [HttpPost("emulate")]
        public IActionResult StartEmulation([FromBody] SAP2CodePacket sap2CodePacket)
        {
            EmulationSessionMap session = null;
            try
            {
                session = _sap1EmuContext.EmulationSessionMaps
                    .AsNoTracking()
                    .Single(esm => esm.EmulationID == sap2CodePacket.EmulationID);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(sap2CodePacket.EmulationID);
            }
            

            // This will save the plain code
            _sap1EmuContext.Add<SAP2CodePacket>(sap2CodePacket);
            _sap1EmuContext.SaveChanges();

            // Assemble 
            SAP2BinaryPacket sap2BinaryPacket = new SAP2BinaryPacket()
            {
                EmulationID = sap2CodePacket.EmulationID,
                Code = Assemble.Parse((List<string>)sap2CodePacket.Code),
                SetName = sap2CodePacket.SetName
            };

            // Save Binary
            _sap1EmuContext.Add(sap2BinaryPacket);

            _sap1EmuContext.SaveChangesAsync(); // Might have to switch to sync

            // RunEmulatorAsync
            RAMProgram rmp = new RAMProgram((List<string>)sap2CodePacket.Code);

            EngineProc engine = new EngineProc();
            engine.Init(rmp, _decoder, sap2BinaryPacket.SetName);
            engine.Run();

            return Ok(engine.FrameStack());
        }

        [HttpPost("{id}/resume")]
        public IActionResult ResumeEmulation([FromBody] string input)
        {
            return Ok();
        }
    }
}
