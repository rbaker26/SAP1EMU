﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using SAP1EMU.GUI.Contexts;
using SAP1EMU.GUI.Hubs;
using SAP1EMU.GUI.Models;
using SAP1EMU.SAP2.Assembler;

using System;
using System.Linq;


namespace SAP1EMU.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SAP2Controller : Controller
    {
        private Sap1EmuContext _sap1EmuContext { get; set; }
        private readonly IHubContext<EmulatorHub> _hubContext;

        public SAP2Controller(Sap1EmuContext sap1EmuContext, IHubContext<EmulatorHub> hubContext)
        {
            _sap1EmuContext = sap1EmuContext;
            _hubContext = hubContext;
        }


        [HttpGet("GetEmulationID")]
        public IActionResult GetEmulationID()
        {
            return Ok(Guid.NewGuid());
        }

        [HttpPost("StartEmulation")]
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

            // Assemble 
            SAP2BinaryPacket sap2BinaryPacket = new SAP2BinaryPacket()
            {
                EmulationID = sap2CodePacket.EmulationID,
                Code = Assemble.Parse((System.Collections.Generic.List<string>)sap2CodePacket.Code)
            };

            // Save Binary
            _sap1EmuContext.Add(sap2BinaryPacket);

            _sap1EmuContext.SaveChangesAsync(); // Might have to switch to sync

            // RunEmulatorAsync
            return Ok();
        }

        [HttpPost("ResumeEmulation")]
        public IActionResult ResumeEmulation([FromBody] string input)
        {
            return Ok();
        }
    }
}
