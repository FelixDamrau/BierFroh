using BierFroh.Modules.KickTipp;
using BierFroh.Modules.Tests.KickTipp.Helper;

namespace BierFroh.Modules.Tests.KickTipp;
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
        var data = TestData.GetTotal();

        var season = await SeasonParser.Parse(data);

        Assert.Equal(14, season.Value.PlayerResults.Count());
    }
}
