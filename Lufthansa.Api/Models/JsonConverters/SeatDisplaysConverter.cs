using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.JsonConverters;

public class SeatDisplaysConverter : JsonConverter<SeatDisplays>
{
    public override SeatDisplays Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var seatDisplays = new SeatDisplays();

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Single SeatDisplay case
            var seatDisplay = JsonSerializer.Deserialize<SeatDisplay>(ref reader, options);
            if (seatDisplay != null) seatDisplays.SeatDisplayList!.Add(seatDisplay);
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Array of SeatDisplay case
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var seatDisplay = JsonSerializer.Deserialize<SeatDisplay>(ref reader, options);
                    if (seatDisplay != null) seatDisplays.SeatDisplayList!.Add(seatDisplay);
                }
        }

        return seatDisplays;
    }

    public override void Write(Utf8JsonWriter writer, SeatDisplays value, JsonSerializerOptions options)
    {
        if (value.SeatDisplayList!.Count == 1)
        {
            // Single SeatDisplay case
            JsonSerializer.Serialize(writer, value.SeatDisplayList[0], options);
        }
        else
        {
            // Array of SeatDisplay case
            writer.WriteStartArray();
            foreach (var seatDisplay in value.SeatDisplayList) JsonSerializer.Serialize(writer, seatDisplay, options);
            writer.WriteEndArray();
        }
    }
}