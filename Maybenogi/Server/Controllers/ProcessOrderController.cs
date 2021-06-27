using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maybenogi.Server.Mabinogi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Maybenogi.Server.Controllers
{
    [Route("process_order")]
    [ApiController]
    public class ProcessOrderController : ControllerBase
    {
        // GET: api/<ProcessOrderController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProcessOrderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProcessOrderController>
        [HttpPost]
        public void Post([FromBody] int value)
        {
            MabiManager.SetProcessOrder(value, true);
        }

        // PUT api/<ProcessOrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProcessOrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
