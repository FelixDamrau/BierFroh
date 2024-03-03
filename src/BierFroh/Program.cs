using BierFroh.State;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;

namespace BierFroh;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddSingleton(sp => new AppStateProvider(sp.GetRequiredService<IJSRuntime>()));
        builder.Services.AddMudServices();
        await builder.Build().RunAsync();
    }
}
