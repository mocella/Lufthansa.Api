using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.JsonConverters;

public class ColumnConverter : JsonConverter<Column>
{
    public override Column Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var column = new Column();

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Handle complex object format
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();

                    if (propertyName!.Equals("Position", StringComparison.OrdinalIgnoreCase))
                        column.Position = JsonSerializer.Deserialize<Position>(ref reader, options);
                    else if (propertyName.Equals("Code", StringComparison.OrdinalIgnoreCase))
                        column.Code = reader.GetString();
                    else
                        reader.Skip();
                }
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            // Handle string format
            column.Code = reader.GetString();
        }
        else
        {
            reader.Skip();
        }

        return column;
    }

    public override void Write(Utf8JsonWriter writer, Column value, JsonSerializerOptions options)
    {
        if (value.Position != null)
        {
            // Write as complex object
            writer.WriteStartObject();

            if (!string.IsNullOrEmpty(value.Code))
            {
                writer.WritePropertyName("Code");
                writer.WriteStringValue(value.Code);
            }

            writer.WritePropertyName("Position");
            JsonSerializer.Serialize(writer, value.Position, options);

            writer.WriteEndObject();
        }
        else if (!string.IsNullOrEmpty(value.Code))
        {
            // Write as simple string
            writer.WriteStringValue(value.Code);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}