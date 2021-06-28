using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Maybenogi.Server.Module;
using Maybenogi.Shared;
using Microsoft.AspNetCore.Cors;
using Maybenogi.Shared.Model;

namespace Maybenogi.Server.Controllers
{
    [EnableCors(Constant.CORS)]
    [Route(ApiRoute.CLIENT)]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ClientContext> Get()
        {
            return SeleniumHandler.Instance.managedClients.Values.ToArray();

            //var processes = Process.GetProcessesByName("client");
            //return processes.Select(process => new ClientContext()
            //{
            //    ProcessId = process.Id,
            //    ProcessName = process.ProcessName,
            //})
            //        .ToArray()
            //    ;
        }
    }
}
