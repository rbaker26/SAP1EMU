using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SAP1EMU.Engine;
using SAP1EMU.Assembler;
using SAP1EMU.Lib;
using Microsoft.EntityFrameworkCore;
using SAP1EMU.GUI.Contexts;
using SAP1EMU.GUI.Models;

namespace SAP1EMU.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmulatorController : ControllerBase
    {
        IDecoder _decoder { get; set; }
        Sap1EmuContext _sap1EmuContext { get; set; }
        public EmulatorController(IDecoder decoder, Sap1EmuContext sap1EmuContext)
        {
            _decoder = decoder;
            _sap1EmuContext = sap1EmuContext;
        }


        public class EmulatorPacket
        {
            public List<string> CodeList { get; set; }
            public string SetName { get; set; }
        }
        // POST: api/Emulator
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] EmulatorPacket emulatorPacket)
        {
            try
            {
                List<string> compiled_binary = Assemble.Parse(emulatorPacket.CodeList, emulatorPacket.SetName);
                RAMProgram rmp = new RAMProgram(compiled_binary);


                EngineProc engine = new EngineProc();
                engine.Init(rmp, _decoder, emulatorPacket.SetName);

                engine.Run();

                _sap1EmuContext.Add<CodeSubmit>(new CodeSubmit
                {
                    code = emulatorPacket.CodeList.Aggregate("", (current, s) => current + (s + ",")),
                    submitted_at = DateTime.Now
                });
                await _sap1EmuContext.SaveChangesAsync();

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


        //// TODO start from here
        //[HttpGet]
        //public string Get(string binCode, string setName)
        //{
        //    return _decoder.Decode(binCode, setName);
        //}

    }
}
