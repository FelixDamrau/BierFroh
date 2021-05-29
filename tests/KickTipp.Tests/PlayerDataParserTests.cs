using System.IO;
using System.Linq;
using KickTippHistory.Core;
using Xunit;

namespace KickTippHistory.Tests.Core
{
    public class PlayerDataParserTests
    {
        [Fact]
        public async void ValidInputGivesValidResult()
        {
            var data = ReadTestData();
            var rawData = await RawPlayerDataParser.GetAllRawPlayerData(data);
            ResultHelper.Validate(rawData.Valid, rawData.ErrorMessage);
            var result = PlayerDataParser.Parse(rawData.Value.First());

            Assert.True(result.Valid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(18)]
        [InlineData(19)]
        [InlineData(20)]
        [InlineData(21)]
        [InlineData(22)]
        [InlineData(23)]
        [InlineData(24)]
        [InlineData(25)]
        [InlineData(26)]
        [InlineData(27)]
        [InlineData(28)]
        [InlineData(29)]
        [InlineData(30)]
        [InlineData(31)]
        [InlineData(32)]
        [InlineData(33)]
        [InlineData(34)]
        public async void TestDataMatchDayPointsAreValid(int matchDay)
        {
            var data = ReadTestData();
            var rawData = await RawPlayerDataParser.GetAllRawPlayerData(data);
            ResultHelper.Validate(rawData.Valid, rawData.ErrorMessage);
            var playerSeasonResult = PlayerDataParser.Parse(rawData.Value.ElementAt(1));
            ResultHelper.Validate(playerSeasonResult.Valid, playerSeasonResult.ErrorMessage);

            var points = playerSeasonResult.Value.GetMatchDayPoints(matchDay);

            Assert.True(points.Valid);
        }

        [Theory]
        [InlineData(1, 13)]
        [InlineData(2, 0)]
        [InlineData(3, 12)]
        [InlineData(4, 10)]
        [InlineData(5, 17)]
        [InlineData(6, 14)]
        [InlineData(7, 13)]
        [InlineData(8, 10)]
        [InlineData(9, 10)]
        [InlineData(10, 12)]
        [InlineData(11, 17)]
        [InlineData(12, 13)]
        [InlineData(13, 12)]
        [InlineData(14, 14)]
        [InlineData(15, 6)]
        [InlineData(16, 4)]
        [InlineData(17, 10)]
        [InlineData(18, 10)]
        [InlineData(19, 15)]
        [InlineData(20, 12)]
        [InlineData(21, 8)]
        [InlineData(22, 11)]
        [InlineData(23, 11)]
        [InlineData(24, 8)]
        [InlineData(25, 9)]
        [InlineData(26, 13)]
        [InlineData(27, 12)]
        [InlineData(28, 12)]
        [InlineData(29, 12)]
        [InlineData(30, 11)]
        [InlineData(31, 16)]
        [InlineData(32, 10)]
        [InlineData(33, 6)]
        [InlineData(34, 15)]
        public async void TestDataMatchDayPointsAreCorrect(int matchDay, int expectedPoints)
        {
            var data = ReadTestData();
            var rawData = await RawPlayerDataParser.GetAllRawPlayerData(data);
            ResultHelper.Validate(rawData.Valid, rawData.ErrorMessage);
            var playerSeasonResult = PlayerDataParser.Parse(rawData.Value.ElementAt(1));

            ResultHelper.Validate(playerSeasonResult.Valid, playerSeasonResult.ErrorMessage);
            var points = playerSeasonResult.Value.GetMatchDayPoints(matchDay);

            Assert.Equal(expectedPoints, points.Value);
        }

        [Theory]
        [InlineData(0, "Felix_FD")]
        [InlineData(1, "Thomas.A")]
        [InlineData(2, "DennisM")]
        [InlineData(3, "StefanE")]
        [InlineData(4, "Stephan")]
        [InlineData(5, "AN")]
        [InlineData(6, "ny")]
        [InlineData(7, "BE")]
        [InlineData(8, "Christoph")]
        [InlineData(9, "Leonidas")]
        [InlineData(10, "Lulu")]
        [InlineData(11, "LW")]
        [InlineData(12, "Tim")]
        [InlineData(13, "TuS_Arminia_MC")]
        public async void TestDataMatchDayPlayerNamesAreCorrect(int playerNumber, string playerName)
        {
            var data = ReadTestData();
            var rawData = await RawPlayerDataParser.GetAllRawPlayerData(data);
            ResultHelper.Validate(rawData.Valid, rawData.ErrorMessage);

            var playerSeasonResult = PlayerDataParser.Parse(rawData.Value.ElementAt(playerNumber));

            ResultHelper.Validate(playerSeasonResult.Valid, playerSeasonResult.ErrorMessage);

            Assert.Equal(playerName, playerSeasonResult.Value.PlayerName);
        }

        private static string ReadTestData()
        {
            return File.ReadAllText("./data/total.html");
        }
    }
}
