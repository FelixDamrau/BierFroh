using AngleSharp.Html.Dom;
using Develix.Essentials.Core;
using BierFroh.Modules.KickTipp.Model;

namespace BierFroh.Modules.KickTipp;
public class PlayerDataParser
{
    public static Result<PlayerSeasonResult> Parse(IHtmlTableRowElement rawPlayerData)
    {
        var playerSeasonResult = ParseInternal(rawPlayerData.Cells);

        return Result.Ok(playerSeasonResult);
    }

    private static PlayerSeasonResult ParseInternal(IEnumerable<IHtmlTableCellElement> rawPlayerData)
    {
        const string dataIndexAttributeName = "data-index";
        const string playerNameClassName = "name";

        var matchDayPoints = new Dictionary<int, int>();
        var playerName = "unknown";
        foreach (var cell in rawPlayerData)
        {
            if (cell.HasAttribute(dataIndexAttributeName))
            {
                if (cell.GetAttribute(dataIndexAttributeName) is string dataIndexValue
                    && int.TryParse(dataIndexValue, out var dataIndex)
                    && !matchDayPoints.ContainsKey(dataIndex)
                    && int.TryParse(cell.TextContent, out var points))
                {
                    matchDayPoints.Add(dataIndex, points);
                }
            }
            else if (cell.ClassList.Contains(playerNameClassName))
            {
                playerName = cell.TextContent;
            }
        }

        var pointsArray = new int?[34];
        foreach (var kvp in matchDayPoints)
        {
            pointsArray[kvp.Key - 1] = kvp.Value;
        }

        return new PlayerSeasonResult(playerName, pointsArray);
    }
}
