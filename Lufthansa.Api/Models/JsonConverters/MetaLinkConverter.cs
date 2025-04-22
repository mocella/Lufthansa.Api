using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.JsonConverters;

public class MetaLinkConverter : JsonConverter<Meta>
{
    public override Meta Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var meta = new Meta();

        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start of object");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName!.Equals("@Version", StringComparison.OrdinalIgnoreCase))
                {
                    meta.Version = reader.GetString();
                }
                else if (propertyName.Equals("Link", StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // Single link case
                        var link = JsonSerializer.Deserialize<Link>(ref reader, options);
                        if (link != null) meta.Links!.Items.Add(link);
                    }
                    else if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        // Array of links case
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            if (reader.TokenType == JsonTokenType.StartObject)
                            {
                                var link = JsonSerializer.Deserialize<Link>(ref reader, options);
                                if (link != null) meta.Links!.Items.Add(link);
                            }
                    }
                }
                else
                {
                    // Skip other properties
                    reader.Skip();
                }
            }

        return meta;
    }

    public override void Write(Utf8JsonWriter writer, Meta value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("@Version");
        writer.WriteStringValue(value.Version);

        writer.WritePropertyName("Link");
        if (value.Links!.Items.Count == 1)
        {
            // Single link case
            JsonSerializer.Serialize(writer, value.Links.Items[0], options);
        }
        else
        {
            // Array of links case
            writer.WriteStartArray();
            foreach (var link in value.Links.Items) JsonSerializer.Serialize(writer, link, options);
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}