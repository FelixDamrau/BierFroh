﻿using System.Text.Json.Serialization;
using Develix.Essentials.Core;

namespace BierFroh.Modules.KickTipp.Model;
[JsonConverter(typeof(PlayerSeasonResultJsonConverter))]
public class PlayerSeasonResult
{
    private readonly IReadOnlyList<int?> matchDayPoints;

    internal PlayerSeasonResult(string playerName, IEnumerable<int?> matchDayPoints)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            throw new ArgumentException($"'{nameof(playerName)}' cannot be null or whitespace.", nameof(playerName));

        if (matchDayPoints is null)
            throw new ArgumentNullException(nameof(matchDayPoints));

        PlayerName = playerName;
        this.matchDayPoints = matchDayPoints.ToList();
    }

    public string PlayerName { get; }

    public Result<int> GetMatchDayPoints(int matchDay)
    {
        if (1 <= matchDay && matchDay <= matchDayPoints.Count)
        {
            if (matchDayPoints[matchDay - 1] is int points)
                return Result.Ok(points);
            else
                return Result.Fail<int>("Match day has no recorded value!");
        }
        return Result.Fail<int>($"'{matchDay}' is not a valid match day!");
    }
}
