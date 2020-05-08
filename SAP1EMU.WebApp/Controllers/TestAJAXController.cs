using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAP1EMU.WebApp.Models;

namespace SAP1EMU.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAJAXController : ControllerBase
    {
        // GET: api/TestAJAX
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TestAJAX/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TestAJAX
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public ActionResult Post([FromForm] InputModel value)
        {
            return Ok(new Tuple<InputModel,string>(value, "Hello World - test test test"));
        }


        // POST: api/TestAJAX
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //    System.Console.Error.WriteLine("YAYAYAYAYAYA");
        //   // System.Console.Error.WriteLine(value);
        //}

        // PUT: api/TestAJAX/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
