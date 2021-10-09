using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace BierFroh.Model;
public static class BrowserFileReader
{
    public static async Task<string> ReadFile(IBrowserFile browserFile)
    {
        using var stream = browserFile.OpenReadStream();
        using var reader = new StreamReader(stream);
        var stringBuilder = new StringBuilder();
        while (await reader.ReadLineAsync() is { } line)
        {
            stringBuilder.AppendLine(line);
        }
        return stringBuilder.ToString();
    }
}
