using System.Text.Json;
using System.Threading.Tasks;
using BierFroh.Common;
using Microsoft.JSInterop;

namespace BierFroh.Model.BrowserStorage
{
    internal class BrowserStorageProvider
    {
        private readonly IJSRuntime jSRuntime;

        public BrowserStorageProvider(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public async ValueTask<Result<AppState>> Get()
        {
            var data = await jSRuntime.InvokeAsync<string>("localStorage.getItem", "AppState");
            if (data is null)
                return Result<AppState>.CreateError("No appstate has been found");

            var appState = System.Text.Json.JsonSerializer.Deserialize<AppState>(data);
            if (appState is null)
                return Result<AppState>.CreateError("Could not deserialize the browser cache data.");
            return Result<AppState>.CreateValid(appState);
        }

        public async ValueTask Set(AppState appState)
        {
            var serializedAppState = JsonSerializer.Serialize(appState);
            await jSRuntime.InvokeVoidAsync("localStorage.setItem", "AppState", serializedAppState);
        }
    }
}
