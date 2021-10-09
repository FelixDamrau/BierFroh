using AngleSharp;
using AngleSharp.Html.Dom;
using Develix.Essentials.Core;

namespace BierFroh.Modules.KickTipp;
public static class RawPlayerDataParser
{
    public static async Task<Result<IEnumerable<IHtmlTableRowElement>>> GetAllRawPlayerData(string data)
    {
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(data));

        var table = document.All.FirstOrDefault(n => n.Id == "ranking");
        if (table is null)
            return Result.Fail<IEnumerable<IHtmlTableRowElement>>("Ranking table could not be found!");

        var body = table.ChildNodes.FirstOrDefault(n => n.NodeName == "TBODY");
        if (body is null)
            return Result.Fail<IEnumerable<IHtmlTableRowElement>>("Body of ranking table could not be found!");

        return Result.Ok(body.ChildNodes.OfType<IHtmlTableRowElement>());
    }
}
