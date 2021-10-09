using BierFroh.Modules.KickTipp;

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
        var data = ReadTestData();

        var season = await SeasonParser.Parse(data);

        Assert.Equal(14, season.Value.PlayerResults.Count());
    }

    private static string ReadTestData()
    {
        return File.ReadAllText("./data/total.html");
    }
}
