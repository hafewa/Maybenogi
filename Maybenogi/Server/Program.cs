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
            SeleniumHandler sh = SeleniumHandler.Instance;

            pwThread = new Thread(new ThreadStart(pw.Update));
            pwThread.Start();

            pw.Subscribe("Client", sh.OnNewProcessFound, sh.OnProcessRemoved);

            CreateHostBuilder(args).Build().Run();
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
