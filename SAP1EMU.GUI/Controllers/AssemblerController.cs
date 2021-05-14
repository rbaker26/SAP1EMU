using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SAP1EMU.Assembler;
using SAP2_Assembler = SAP1EMU.SAP2.Assembler;

using SAP1_Lib = SAP1EMU.Lib;
using SAP2_Lib = SAP1EMU.SAP2.Lib;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAP1EMU.GUI.Controllers
{
    
    [Route("api/assembler")]
    [ApiController]
    public class AssemblerController : ControllerBase
    {
        /// <summary>
        /// Gets a list of supported instruction sets for the specified emulator
        /// </summary>
        /// <returns>A list of instruction sets</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="500">List not found. Contact Network Admin</response>    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("supported_sets")]
        public IActionResult GetSupportedSets([FromQuery] string emulator)
        {
            try
            {
                if (emulator.Equals("SAP1"))
                {
                    return Ok(SAP1_Lib.OpCodeLoader.GetISetNames());
                }
                else if (emulator.Equals("SAP2"))
                {
                    return Ok(SAP2_Lib.OpCodeLoader.GetISetNames());
                }

                //Not a set we have trigger that catch block below
                throw new Exception();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("supported_emulators")]
        public IActionResult GetEmulators()
        {
            try
            {
                return Ok(new List<string>() { "SAP1", "SAP2" });

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        public class AssemblePacket
        {
            [Required]
            public List<string> CodeList { get; set; }

            [Required]
            public string SetName { get; set; }

            [Required]
            public string Emulator { get; set; }
        }

        /// <summary>
        /// Compiles SAP1Emu compatable code into Binary
        /// </summary>
        /// <param name="assemblePacket">A packet of code and the set name</param>
        /// <returns>A list of binary strings</returns>
        /// <response code="200">Returns the list of binary code</response>
        /// <response code="400">Code contained syntax errors or sets does not exist</response> 
        /// <response code="500">Server Error. Contact Network Admin</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult Post([FromBody] AssemblePacket assemblePacket)
        {
            try
            {
                if(assemblePacket.Emulator.Equals("SAP1"))
                {
                    return Ok(Assemble.Parse(assemblePacket.CodeList, assemblePacket.SetName));
                }
                else if (assemblePacket.Emulator.Equals("SAP2"))
                {
                    return Ok(SAP2_Assembler.Assemble.Parse(assemblePacket.CodeList, assemblePacket.SetName));
                }

                //Not a set we have trigger that catch block below
                throw new Exception();
            }
            catch (ParseException pe)
            {
                return BadRequest(pe.Message + " " + pe.InnerException.Message);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}