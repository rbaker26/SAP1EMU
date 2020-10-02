using Microsoft.AspNetCore.Mvc;

using SAP1EMU.Assembler;
using SAP1EMU.Lib;

using System;
using System.Collections.Generic;

namespace SAP1EMU.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssemblerController : ControllerBase
    {
        // GET: api/Assembler
        [HttpGet("supported_sets")]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                return Ok(OpCodeLoader.GetISetNames());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " " + e.InnerException.Message);
            }
        }

        public class AssemblePacket
        {
            public List<string> CodeList { get; set; }
            public string SetName { get; set; }
        }

        // POST: api/Assembler
        [HttpPost]
        public ActionResult Post([FromBody] AssemblePacket assemblePacket)
        {
            try
            {
                return Ok(Assemble.Parse(assemblePacket.CodeList, assemblePacket.SetName));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " " + e.InnerException.Message);
            }
        }
    }
}