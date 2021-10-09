using System.Text;
using System.Text.Json;
using BierFroh.Modules.KickTipp;
using BierFroh.Modules.KickTipp.Model;

namespace BierFroh.Modules.Tests.KickTipp;
public class PlayerSeasonResultSerializationTests
{
    [Fact]
    public void CustomSerializationIsUsed()
    {
        var playerSeasonResult = new PlayerSeasonResult("Name", new int?[] { 1, 2, 3 });
        var customSerializer = new PlayerSeasonResultJsonConverter();
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream);
        customSerializer.Write(writer, playerSeasonResult, new JsonSerializerOptions());
        var expectedSerializedResult = Encoding.UTF8.GetString(stream.ToArray());

        var serializedResult = JsonSerializer.Serialize(playerSeasonResult);

        Assert.Equal(expectedSerializedResult, serializedResult);
    }
}
