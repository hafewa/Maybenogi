using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Maybenogi.Server.Mabinogi;
using Maybenogi.Server.Module;
using Maybenogi.Shared;
using Maybenogi.Shared.Model;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Maybenogi.Server.Controllers
{
    [Route(ApiRoute.OPTIMIZATION)]
    [EnableCors(Constant.CORS)]
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
            foreach (var client in SeleniumHandler.Instance.managedClients.Values)
            {
                if (client.ProcessId == value)
                {
                    MabiManager.SetProcessOrder(value, true);
                    client.ClientState = EClientState.Main;
                }
                else
                {
                    MabiManager.SetProcessOrder(value, false);
                    client.ClientState = EClientState.Sub;
                }
            }
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
