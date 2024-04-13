using Microsoft.AspNetCore.Components.Forms;

namespace BierFroh.Model;

public static class BrowserFileReader
{
    public static async Task<string> ReadFile(IBrowserFile browserFile) 
        => await ReadFile(browserFile, 512000);

    public static async Task<string> ReadFile(IBrowserFile browserFile, long maxAllowedSize)
    {
        using var stream = browserFile.OpenReadStream(maxAllowedSize);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
