using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.JsonConverters;

public class NamesConverter : JsonConverter<Names>
{
    public override Names Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var names = new Names();

        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start of object");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName!.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // Single name case
                        var name = JsonSerializer.Deserialize<Name>(ref reader, options);
                        if (name != null) names.NameList.Add(name);
                    }
                    else if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        // Array of names case
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            if (reader.TokenType == JsonTokenType.StartObject)
                            {
                                var name = JsonSerializer.Deserialize<Name>(ref reader, options);
                                if (name != null) names.NameList.Add(name);
                            }
                    }
                }
                else
                {
                    // Skip other properties
                    reader.Skip();
                }
            }

        return names;
    }

    public override void Write(Utf8JsonWriter writer, Names value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Name");
        if (value.NameList.Count == 1)
        {
            // Single name case
            JsonSerializer.Serialize(writer, value.NameList[0], options);
        }
        else
        {
            // Array of names case
            writer.WriteStartArray();
            foreach (var name in value.NameList) JsonSerializer.Serialize(writer, name, options);
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}