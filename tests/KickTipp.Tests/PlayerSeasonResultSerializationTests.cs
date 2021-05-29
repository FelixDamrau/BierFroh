using System.IO;
using System.Text;
using System.Text.Json;
using KickTippHistory.Core;
using KickTippHistory.Core.Model;
using Xunit;

namespace KickTippHistory.Tests.Core
{
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

            var serializedResult = JsonSerializer.Serialize<PlayerSeasonResult>(playerSeasonResult);

            Assert.Equal(expectedSerializedResult, serializedResult);
        }
    }
}
