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
            try
            {
                List<string> compiled_binary = Assemble.Parse(emulatorPacket.CodeList, emulatorPacket.SetName);
                RAMProgram rmp = new RAMProgram(compiled_binary);

                EngineProc engine = new EngineProc();
                engine.Init(rmp, _decoder, emulatorPacket.SetName);
                engine.Run();

                // TODO: Dispatch to seperate thread
                try
                {
                    _sap1EmuContext.Add<CodeSubmit>(new CodeSubmit
                    {
                        code = emulatorPacket.CodeList.Aggregate("", (current, s) => current + (s + ",")),
                        submitted_at = DateTime.Now
                    });
                    await _sap1EmuContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // TODO: Log DB Error
                }

                return Ok(engine.FrameStack());
            }
            catch(ParseException pe)
            {
                if (pe.InnerException != null)
                {
                    return BadRequest(pe.Message + " " + pe.InnerException.Message);
                }
                else
                {
                    return BadRequest(pe.Message);
                }
            }
            catch(EngineRuntimeException ere)
            {
                if (ere.InnerException != null)
                {
                    return BadRequest(ere.Message + " " + ere.InnerException.Message);
                }
                else
                {
                    return BadRequest(ere.Message);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}