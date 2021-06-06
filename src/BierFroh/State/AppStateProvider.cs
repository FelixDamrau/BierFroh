using System.Text.Json;
using System.Threading.Tasks;
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

        public async ValueTask<AppState> Get()
        {
            var data = await jSRuntime.InvokeAsync<string>("localStorage.getItem", "AppState");
            if (data is null)
                return AppState.Default;

            var appState = System.Text.Json.JsonSerializer.Deserialize<AppState>(data);
            if (appState is null)
                return AppState.Default;

            var validatedAppState = Validate(appState);

            return validatedAppState;
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
            var appState = await Get();
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
