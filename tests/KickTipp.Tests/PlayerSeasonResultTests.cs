using System;
using KickTippHistory.Core.Model;
using Xunit;

namespace KickTippHistory.Tests.Core
{
    public class PlayerSeasonResultTests
    {
        [Fact]
        public void GetMatchDayPointsIsValid()
        {
            var points = new int?[] { 1, 2, 3 };
            var playerSeasonResult = new PlayerSeasonResult("name", points);
            var matchDay = 2;

            var result = playerSeasonResult.GetMatchDayPoints(matchDay);

            Assert.True(result.Valid);
        }

        [Fact]
        public void GetMatchDayPoints()
        {
            var points = new int?[] { 1, 2, 3 };
            var playerSeasonResult = new PlayerSeasonResult("name", points);
            var matchDay = 2;

            var result = playerSeasonResult.GetMatchDayPoints(matchDay);

            Assert.Equal(points[matchDay - 1], result.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void GetInvalidMatchDayPointsIsNotValid(int matchDay)
        {
            var points = new int?[] { 1, 2, 3 };
            var playerSeasonResult = new PlayerSeasonResult("name", points);

            var result = playerSeasonResult.GetMatchDayPoints(matchDay);

            Assert.False(result.Valid);
        }

        [Fact]
        public void GetNotDataMatchDayPointsIsNotValid()
        {
            var points = new int?[] { 1, null, 3 };
            var playerSeasonResult = new PlayerSeasonResult("name", points);

            var result = playerSeasonResult.GetMatchDayPoints(2);

            Assert.False(result.Valid);
        }

        [Fact]
        public void PassedPlayerNameIsCorrect()
        {
            var points = System.Array.Empty<int?>();
            var playerName = "Name";
            var playerSeasonResult = new PlayerSeasonResult(playerName, points);

            Assert.Equal(playerName, playerSeasonResult.PlayerName);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("\t")]
        public void NullOrWhiteSpacePlayerNameThrowsArgumentException(string? invalidPlayerName)
        {
            var points = System.Array.Empty<int?>();
            var exception = Record.Exception(() => new PlayerSeasonResult(invalidPlayerName!, points));

            Assert.IsType<ArgumentException>(exception);
        }
    }
}
