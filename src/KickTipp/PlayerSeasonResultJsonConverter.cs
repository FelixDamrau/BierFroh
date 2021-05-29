using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using KickTippHistory.Core.Model;

namespace KickTippHistory.Core
{
    public class PlayerSeasonResultJsonConverter : JsonConverter<PlayerSeasonResult>
    {
        internal const string MatchDayPointsJsonPropertyName = "MatchDayPoints";
        public override PlayerSeasonResult? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, PlayerSeasonResult value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(PlayerSeasonResult.PlayerName), value.PlayerName);

            writer.WritePropertyName(MatchDayPointsJsonPropertyName);
            writer.WriteStartArray();
            for (int matchDay = 1; matchDay <= 34; ++matchDay)
            {
                var matchDayPoints = value.GetMatchDayPoints(matchDay);
                if (matchDayPoints.Valid)
                    writer.WriteNumberValue(matchDayPoints.Value);
                else
                    writer.WriteNullValue();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Flush();
        }

        public override bool HandleNull => true;
    }
}
