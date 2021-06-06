using System;
using System.Net.Http;
using System.Threading.Tasks;
using BierFroh.Model.BrowserStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using MudBlazor.Services;

namespace BierFroh
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(sp => new BrowserStorageProvider(sp.GetRequiredService<IJSRuntime>()));
            builder.Services.AddMudServices();
            await builder.Build().RunAsync();
        }
    }
}
