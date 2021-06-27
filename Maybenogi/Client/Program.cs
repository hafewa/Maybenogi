using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Maybenogi.Client.Services;

namespace Maybenogi.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
                .AddScoped<IAlertService, AlertService>()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped(sp =>
            {
                var httpClient = new HttpClient
                {
                    //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                    BaseAddress = new Uri("http://localhost:5000")
                };

                return httpClient;
            });

            await builder.Build().RunAsync();
        }
    }
}
