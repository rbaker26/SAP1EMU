using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SAP1EMU.Engine;
using SAP1EMU.Assembler;
using SAP1EMU.Lib;

namespace SAP1EMU.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmulatorController : ControllerBase
    {

        public class EmulatorPacket
        {
            public List<string> CodeList { get; set; }
            public string SetName { get; set; }
        }
        // POST: api/Emulator
        [HttpPost]
        public ActionResult Post([FromBody] EmulatorPacket emulatorPacket)
        {
            try
            {
                List<string> compiled_binary = Assemble.Parse(emulatorPacket.CodeList, emulatorPacket.SetName);
                RAMProgram rmp = new RAMProgram(compiled_binary);


                EngineProc engine = new EngineProc();
                engine.Init(rmp, emulatorPacket.SetName);

                engine.Run();

                return Ok(engine.FrameStack());

            }
            catch (Exception e)
            {
                if(e.InnerException != null)
                {
                    return BadRequest(e.Message + " " + e.InnerException.Message);
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
        }

    }
}
