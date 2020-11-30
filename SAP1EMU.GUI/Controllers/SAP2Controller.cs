using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using SAP1EMU.GUI.Contexts;
using SAP1EMU.GUI.Hubs;
using SAP1EMU.GUI.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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

        // TODO - Remove this after testing
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("GetEmulationID")]
        public IActionResult GetEmulationID()
        {
            return Ok(Guid.NewGuid());
        }

        [HttpPost("StartEmulation")]
        public IActionResult StartEmulation([FromBody] SAP2CodePacket sap2CodePacket)
        {
            var session = _sap1EmuContext.EmulationSessionMaps
                .AsNoTracking()
                .Single(esm => esm.EmulationID == sap2CodePacket.EmulationID);
            if(session == null)
            {
                return NotFound(sap2CodePacket.EmulationID);
            }
            
            return Ok();
        }

    }
}
