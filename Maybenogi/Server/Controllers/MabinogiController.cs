using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Maybenogi.Server.Module;
using Maybenogi.Shared;
using Maybenogi.Shared.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Maybenogi.Server.Controllers
{
    [EnableCors(Constant.CORS)]
    [Route(ApiRoute.BOOTSTRAP)]
    [ApiController]
    public class MabinogiController : ControllerBase
    {
        [HttpGet("launch/{accountId:int}")]
        public async Task<ClientContext> Create(int accountId)
        {
            var clientContext = new ClientContext();
            
            var di = new DirectoryInfo("accounts");
            if (!di.Exists)
                di.Create();

            var fiPath = di.FullName + $"/{accountId:00000000}.nxmbng";
            var fi = new FileInfo(fiPath);

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

        [HttpGet("dispose/{processId:int}")]
        public async Task<bool> Dispose(int processId)
        {
            try
            {
                Process.GetProcessById(processId)?.Kill(true);

                if (SeleniumHandler.Instance.managedClients.TryGetValue(processId, out var client))
                {
                    SeleniumHandler.Instance.managedClients.Remove(processId);
                }

                await Task.Delay(100);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] {ex.Message}");
            }
            
            return false;
        }
    }
}
