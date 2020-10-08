using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SAP1EMU.Assembler;
using SAP1EMU.Lib;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAP1EMU.GUI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AssemblerController : ControllerBase
    {
        /// <summary>
        /// Gets a list of supported instruction sets
        /// </summary>
        /// <returns>A list of instruction sets</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="500">List not found. Contact Network Admin</response>    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("supported_sets")]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                return Ok(OpCodeLoader.GetISetNames());
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
        public ActionResult Post([FromBody] AssemblePacket assemblePacket)
        {
            try
            {
                return Ok(Assemble.Parse(assemblePacket.CodeList, assemblePacket.SetName));
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