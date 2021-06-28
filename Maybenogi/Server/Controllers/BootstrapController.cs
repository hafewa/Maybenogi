using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Maybenogi.Server.Module;
using Maybenogi.Shared.Model;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace Maybenogi.Server.Controllers
{
    [EnableCors(Constant.CORS)]
    [Route("api/launch")]
    [ApiController]
    public class BootstrapController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ClientContext> Get(int id)
        {
            var clientContext = new ClientContext();
            
            var di = new DirectoryInfo("accounts");
            if (!di.Exists)
                di.Create();

            var fi = new FileInfo(di.FullName + $"/{id:00000000}.nxmbng");
            if (!fi.Exists) return clientContext;

            var json = await System.IO.File.ReadAllTextAsync(fi.FullName);
            var account = JsonConvert.DeserializeObject<NexonAccount>(json);

            if (account == null) return clientContext;

            await Task.Delay(100);
            var result = await SeleniumHandler.Instance.CreateClient(account);

            clientContext.ProcessId = result.Id;
            clientContext.ProcessName = result.ProcessName;
            clientContext.AccountId = account.UID;
            clientContext.AccountName = account.DisplayName;

            SeleniumHandler.Instance.managedClients[result.Id] = clientContext;

            return clientContext;
        }
    }
}
