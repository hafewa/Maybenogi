using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Maybenogi.Shared.Model;

namespace Maybenogi.Server.Controllers
{
    [EnableCors(Constant.CORS)]
    [Route("current_clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ClientContext> Get()
        {
            var processes = Process.GetProcessesByName("client");
            return processes.Select(process => new ClientContext()
            {
                ProcessId = process.Id,
                ProcessName = process.ProcessName,
            })
                    .ToArray()
                ;
        }
    }
}
