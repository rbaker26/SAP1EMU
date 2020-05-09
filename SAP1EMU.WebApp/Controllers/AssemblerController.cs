using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAP1EMU.Assembler;

namespace SAP1EMU.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssemblerController : ControllerBase
    {
        //// GET: api/Assembler
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Assembler/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Assembler
        [HttpPost]
        public ActionResult Post([FromBody] List<string> codeList)
        {
            try
            {
                return Ok(SAP1EMU.Assembler.Assemble.ParseFileContents(codeList));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " " + e.InnerException.Message);
            }
        }

        //// PUT: api/Assembler/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
