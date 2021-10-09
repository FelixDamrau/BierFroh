using System.Text.Json;
using BierFroh.Modules.KickTipp;
using BierFroh.Modules.KickTipp.Model;
using BierFroh.Modules.Tests.KickTipp.Helper;
using Xunit;

namespace BierFroh.Modules.Tests.KickTipp;
public class PlayerSeasonResultJsonConverterTests
{
    [Fact]
    public void CanConvertPlayerSeasonResult()
    {
        var jsonConverter = new PlayerSeasonResultJsonConverter();

        var canConvert = jsonConverter.CanConvert(typeof(PlayerSeasonResult));

        Assert.True(canConvert);
    }

    [Fact]
    public void CanHandleNull()
    {
        var jsonConverter = new PlayerSeasonResultJsonConverter();

        Assert.True(jsonConverter.HandleNull);
    }

    [Fact]
    public void SerializesPlayerName()
    {
        var jsonConverter = new PlayerSeasonResultJsonConverter();
        var stream = new MemoryStream();
        var jsonWriter = new Utf8JsonWriter(stream);
        var playerName = "Name";
        var result = new PlayerSeasonResult(playerName, Enumerable.Empty<int?>());


        jsonConverter.Write(jsonWriter, result, new JsonSerializerOptions());

        var jsonText = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var jsonDocument = JsonDocument.Parse(jsonText);
        var playerNamePropertySerialized = jsonDocument.RootElement.TryGetProperty(nameof(PlayerSeasonResult.PlayerName), out _);
        Assert.True(playerNamePropertySerialized);
    }

    [Fact]
    public void SerializesPlayerNameValue()
    {
        var jsonConverter = new PlayerSeasonResultJsonConverter();
        var stream = new MemoryStream();
        var jsonWriter = new Utf8JsonWriter(stream);
        var playerName = "Name";
        var result = new PlayerSeasonResult(playerName, Enumerable.Empty<int?>());


        jsonConverter.Write(jsonWriter, result, new JsonSerializerOptions());
        var jsonText = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var jsonDocument = JsonDocument.Parse(jsonText);

        var jsonElement = jsonDocument.RootElement.GetProperty(nameof(PlayerSeasonResult.PlayerName));
        var jsonElementValue = jsonElement.GetString();
        Assert.Equal(playerName, jsonElementValue);
    }

    [Fact]
    public void SerializesMatchDayPoints()
    {
        var jsonConverter = new PlayerSeasonResultJsonConverter();
        var stream = new MemoryStream();
        var jsonWriter = new Utf8JsonWriter(stream);
        var matchDayPoints = new int?[] { 1, 2, 3 };
        var result = new PlayerSeasonResult("Foo", matchDayPoints);


        jsonConverter.Write(jsonWriter, result, new JsonSerializerOptions());

        var jsonText = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var jsonDocument = JsonDocument.Parse(jsonText);
        var matchDayPointsSerialized = jsonDocument.RootElement.TryGetProperty(PlayerSeasonResultJsonConverter.MatchDayPointsJsonPropertyName, out _);
        Assert.True(matchDayPointsSerialized);
    }

    [Fact]
    public void SerializesMatchDayPointsValue()
    {
        var jsonConverter = new PlayerSeasonResultJsonConverter();
        var stream = new MemoryStream();
        var jsonWriter = new Utf8JsonWriter(stream);
        var matchDayPoints = new int?[] { 1, 2, 3 };
        var result = new PlayerSeasonResult("Foo", matchDayPoints);

        jsonConverter.Write(jsonWriter, result, new JsonSerializerOptions());

        var jsonText = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        var jsonDocument = JsonDocument.Parse(jsonText);
        var matchDayPointsJsonElement = jsonDocument.RootElement.GetProperty(PlayerSeasonResultJsonConverter.MatchDayPointsJsonPropertyName);
        var deserializedMatchDayPoints = matchDayPointsJsonElement.EnumerateArray().Select<JsonElement, int?>(j => j.ValueKind == JsonValueKind.Null ? null : j.GetInt32());
        var deserializedMatchDayPointstWithOutTrailingNulls = EnumerableHelper.TrimTrailingNullValues(deserializedMatchDayPoints);

        Assert.Equal(matchDayPoints, deserializedMatchDayPointstWithOutTrailingNulls);
    }
}
