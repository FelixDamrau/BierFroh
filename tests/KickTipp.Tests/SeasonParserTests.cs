using System.IO;
using System.Linq;
using KickTippHistory.Core;
using Xunit;

namespace KickTippHistory.Tests.Core
{
    public class SeasonParserTests
    {
        [Fact]
        public async void EmptyInputYieldsInvalidSeason()
        {
            var season = await SeasonParser.Parse(string.Empty);

            Assert.False(season.Valid);
        }

        [Fact]
        public async void TestDataYields14PlayerSeasonResults()
        {
            var data = ReadTestData();

            var season = await SeasonParser.Parse(data);
            ResultHelper.Validate(season.Valid, season.ErrorMessage);

            Assert.Equal(14, season.Value.PlayerResults.Count());
        }

        private static string ReadTestData()
        {
            return File.ReadAllText("./data/total.html");
        }
    }
}
