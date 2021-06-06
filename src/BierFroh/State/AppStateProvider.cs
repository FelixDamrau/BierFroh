using System.Text.Json;
using System.Threading.Tasks;
using BierFroh.Common;
using Microsoft.JSInterop;

namespace BierFroh.State
{
    internal class AppStateProvider
    {
        private readonly IJSRuntime jSRuntime;

        public AppStateProvider(IJSRuntime jSRuntime)
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

            var validatedAppState = Validate(appState);

            return Result<AppState>.CreateValid(validatedAppState);
        }

        private static AppState Validate(AppState appState)
        {
            if (appState.Version != AppState.ProgramVersion)
                return AppState.Default;

            var validatedAppState = appState;
            if (appState.RootState.Version != RootState.ProgramVersion)
                validatedAppState = validatedAppState with { RootState = RootState.Default };

            return validatedAppState;
        }

        public async ValueTask Update(RootState rootState)
        {
            var appStateResult = await Get();
            if (!appStateResult.Valid)
                return;

            var appState = appStateResult.Value;
            var newAppState = appState with { RootState = rootState };
            await Set(newAppState);
        }

        private async ValueTask Set(AppState appState)
        {
            var serializedAppState = JsonSerializer.Serialize(appState);
            await jSRuntime.InvokeVoidAsync("localStorage.setItem", "AppState", serializedAppState);
        }
    }
}
