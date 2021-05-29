using System.Collections.Generic;
using System.Threading.Tasks;
using KickTippHistory.Core.Model;

namespace KickTippHistory.Core
{
    public static class SeasonParser
    {
        public static async Task<Result<Season>> Parse(string rawData)
        {
            if (string.IsNullOrWhiteSpace(rawData))
                return Result<Season>.CreateError("The season could not be parsed!");

            var playerData = await RawPlayerDataParser.GetAllRawPlayerData(rawData);
            if (!playerData.Valid)
                return Result<Season>.CreateError("The season could not be parsed!");

            var playerSeasonResults = new List<PlayerSeasonResult>();
            foreach (var data in playerData.Value)
            {
                var playerSeasonResult = PlayerDataParser.Parse(data);
                if (playerSeasonResult.Valid)
                    playerSeasonResults.Add(playerSeasonResult.Value);
            }

            return Result<Season>.CreateValid(new Season { PlayerResults = playerSeasonResults });
        }
    }
}
