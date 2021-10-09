using Develix.Essentials.Core;
using BierFroh.Modules.KickTipp.Model;

namespace BierFroh.Modules.KickTipp;
public static class SeasonParser
{
    public static async Task<Result<Season>> Parse(string rawData)
    {
        if (string.IsNullOrWhiteSpace(rawData))
            return Result.Fail<Season>("The season could not be parsed!");

        var playerData = await RawPlayerDataParser.GetAllRawPlayerData(rawData);
        if (!playerData.Valid)
            return Result.Fail<Season>("The season could not be parsed!");

        var playerSeasonResults = new List<PlayerSeasonResult>();
        foreach (var data in playerData.Value)
        {
            var playerSeasonResult = PlayerDataParser.Parse(data);
            if (playerSeasonResult.Valid)
                playerSeasonResults.Add(playerSeasonResult.Value);
        }

        return Result.Ok(new Season { PlayerResults = playerSeasonResults });
    }
}
