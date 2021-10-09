using System.Text.Json;
using BierFroh.Modules.KickTipp.Model;

namespace BierFroh.Modules.Tests.KickTipp;
public class SeasonSerializationTests
{
    [Fact]
    public void CanSerializeSeason()
    {
        var season = new Season
        {
            PlayerResults = new[]
            {
                new PlayerSeasonResult("Foo", new int?[] { 1, 2, 3 }),
                new PlayerSeasonResult("Bar", new int?[] { 4, null, 6 })
            }
        };
        void Serialize() => JsonSerializer.Serialize<Season>(season);

        var exception = Record.Exception(Serialize);

        Assert.Null(exception);
    }
}
