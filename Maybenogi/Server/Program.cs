using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;

namespace Maybenogi.Server
{
    using Module;

    public class Program
    {
        public static Thread pwThread;

        public static void Main(string[] args)
        {
            ProcessWatcher pw = ProcessWatcher.Instance;
            pwThread = new Thread(new ThreadStart(pw.Update));
            pwThread.Start();

            pw.Subscribe("Client", onAttach, onDetach);

            CreateHostBuilder(args).Build().Run();

            void onAttach(Process proc)
            {
                Console.WriteLine($"Attached!! : {proc.Id}");
            }

            void onDetach(int pid)
            {
                Console.WriteLine($"Detached!! : {pid}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
