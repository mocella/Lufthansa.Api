using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.JsonConverters;

public class CharacteristicCollectionConverter : JsonConverter<List<Characteristic>>
{
    public override List<Characteristic> Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var characteristics = new List<Characteristic>();

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Single characteristic case
            var characteristic = JsonSerializer.Deserialize<Characteristic>(ref reader, options);
            if (characteristic != null) characteristics.Add(characteristic);
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Array of characteristics case
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var characteristic = JsonSerializer.Deserialize<Characteristic>(ref reader, options);
                    if (characteristic != null) characteristics.Add(characteristic);
                }
        }

        return characteristics;
    }

    public override void Write(Utf8JsonWriter writer, List<Characteristic> value, JsonSerializerOptions options)
    {
        if (value.Count == 1)
        {
            // Single characteristic case
            JsonSerializer.Serialize(writer, value[0], options);
        }
        else
        {
            // Array of characteristics case
            writer.WriteStartArray();
            foreach (var characteristic in value) JsonSerializer.Serialize(writer, characteristic, options);
            writer.WriteEndArray();
        }
    }
}