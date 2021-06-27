using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Maybenogi.Shared.Utils;

namespace Maybenogi.Server.Module
{
    public class ProcessWatcher : Singleton<ProcessWatcher>
    {
        public readonly Dictionary<string, Dictionary<int, Process>> managedProcesses =
            new Dictionary<string, Dictionary<int, Process>>();

        public void Update()
        {
            while (true)
            {
                foreach (var processName in _subscribeTargets)
                {
                    var processes = Process.GetProcessesByName(processName);
                    var previousProcessMap = managedProcesses[processName];

                    foreach (var proc in processes)
                    {
                        if (previousProcessMap.ContainsKey(proc.Id))
                        {
                            continue;
                        }

                        previousProcessMap[proc.Id] = proc;
                        _onProcessAttach[processName]?.Invoke(proc);
                    }

                    var pids = processes.Select(p => p.Id).ToArray();
                    foreach (var pair in previousProcessMap)
                    {
                        var key = pair.Key;
                        if (pids.Contains(key))
                        {
                            continue;
                        }

                        previousProcessMap.Remove(key);
                        _onProcessDetach[processName]?.Invoke(key);
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private readonly HashSet<string> _subscribeTargets = new HashSet<string>();

        private readonly Dictionary<string, Action<Process>> _onProcessAttach = new Dictionary<string, Action<Process>>();
        private readonly Dictionary<string, Action<int>> _onProcessDetach = new Dictionary<string, Action<int>>();

        public void Subscribe(string processName, Action<Process> attachCallback, Action<int> detachCallback)
        {
            if (_subscribeTargets.Contains(processName)) return;

            _subscribeTargets.Add(processName);

            this._onProcessAttach[processName] = attachCallback;
            this._onProcessDetach[processName] = detachCallback;

            managedProcesses[processName] = new Dictionary<int, Process>();
        }
    }
}
