using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Reference;

namespace Lufthansa.Api.Models.JsonConverters;

public class AirlinesConverter : JsonConverter<Airlines>
{
    public override Airlines Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var airlines = new Airlines();

        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start of object");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName!.Equals("Airline", StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // Single airline case
                        var airline = JsonSerializer.Deserialize<Airline>(ref reader, options);
                        if (airline != null) airlines.AirlineList!.Add(airline);
                    }
                    else if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        // Array of airlines case
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            if (reader.TokenType == JsonTokenType.StartObject)
                            {
                                var airline = JsonSerializer.Deserialize<Airline>(ref reader, options);
                                if (airline != null) airlines.AirlineList!.Add(airline);
                            }
                    }
                }
                else
                {
                    // Skip other properties
                    reader.Skip();
                }
            }

        return airlines;
    }

    public override void Write(Utf8JsonWriter writer, Airlines value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Airline");
        if (value.AirlineList!.Count == 1)
        {
            // Single airline case
            JsonSerializer.Serialize(writer, value.AirlineList[0], options);
        }
        else
        {
            // Array of airlines case
            writer.WriteStartArray();
            foreach (var airline in value.AirlineList) JsonSerializer.Serialize(writer, airline, options);
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}