using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Maybenogi.Shared.Model;
using Maybenogi.Shared.Utils;
using Newtonsoft.Json;

namespace Maybenogi.Server.Module
{
    public class SeleniumHandler : Singleton<SeleniumHandler>
    {
        private bool isWaitingNewProcess;
        private Process catchedProcess;

        private Option _option;

        public Option Option
        {
            get
            {
                if (_option == null)
                {
                    var di = new DirectoryInfo("settings");
                    if (!di.Exists)
                        di.Create();

                    var fi = new FileInfo("settings/setting.json");
                    if (!fi.Exists)
                    {
                        _option = new Option();
                        var json = JsonConvert.SerializeObject(_option);

                        System.IO.File.WriteAllText(fi.FullName, json);
                    }
                    else
                    {
                        var json = System.IO.File.ReadAllText(fi.FullName);
                        _option = JsonConvert.DeserializeObject<Option>(json);
                    }
                }

                return _option;
            }
        }
        public readonly Dictionary<int, ClientContext> managedClients = new Dictionary<int, ClientContext>();

        protected override void ctor()
        {

        }

        public void SetOption(Option option)
        {
            this._option = option;
        }

        public async Task<Process> CreateClient(NexonAccount account)
        {
            await Task.Delay(10);

            var builder = new Session.Builder();

            var session = builder
                .SetAccount(account)
                .SetBrowser(Option.BrowserType)
                .SetHeadless(Option.Headless)
                .SetResolution(Option.BrowserWidth, Option.BrowserHeight)
                .Build();

            await session.Run();

            isWaitingNewProcess = true;
            Process process = await WaitProcess();
            catchedProcess = null;
            isWaitingNewProcess = false;

            await session.Dispose();
            await Task.Delay(100);

            return process;
        }

        private async Task<Process> WaitProcess()
        {
            await Task.Run(() =>
            {
                while (catchedProcess == null)
                    Thread.Sleep(10);
            });
            
            return catchedProcess;
        }

        public void OnNewProcessFound(Process process)
        {
            if (isWaitingNewProcess)
            {
                catchedProcess = process;
            }
        }

        public void OnProcessRemoved(int processId)
        {
            if (managedClients.TryGetValue(processId, out var client))
            {
                managedClients.Remove(processId);
            }
        }
    }
}
