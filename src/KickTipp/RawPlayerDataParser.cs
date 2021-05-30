using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;
using BierFroh.Common;

namespace KickTippHistory.Core
{
    public static class RawPlayerDataParser
    {
        public static async Task<Result<IEnumerable<IHtmlTableRowElement>>> GetAllRawPlayerData(string data)
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(data));

            var table = document.All.FirstOrDefault(n => n.Id == "ranking");
            if (table is null)
                return Result<IEnumerable<IHtmlTableRowElement>>.CreateError("Ranking table could not be found!");

            var body = table.ChildNodes.FirstOrDefault(n => n.NodeName == "TBODY");
            if (body is null)
                return Result<IEnumerable<IHtmlTableRowElement>>.CreateError("Body of ranking table could not be found!");

            return Result<IEnumerable<IHtmlTableRowElement>>.CreateValid(body.ChildNodes.OfType<IHtmlTableRowElement>());
        }
    }
}
