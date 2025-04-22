using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.JsonConverters;

public class AirportsConverter : JsonConverter<Airports>
{
    public override Airports Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var airports = new Airports();

        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start of object");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName!.Equals("Airport", StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // Single airport case
                        var airport = JsonSerializer.Deserialize<Airport>(ref reader, options);
                        if (airport != null) airports.AirportList!.Add(airport);
                    }
                    else if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        // Array of airports case
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            if (reader.TokenType == JsonTokenType.StartObject)
                            {
                                var airport = JsonSerializer.Deserialize<Airport>(ref reader, options);
                                if (airport != null) airports.AirportList!.Add(airport);
                            }
                    }
                }
                else
                {
                    // Skip other properties
                    reader.Skip();
                }
            }

        return airports;
    }

    public override void Write(Utf8JsonWriter writer, Airports value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Airport");
        if (value.AirportList!.Count == 1)
        {
            // Single airport case
            JsonSerializer.Serialize(writer, value.AirportList[0], options);
        }
        else
        {
            // Array of airports case
            writer.WriteStartArray();
            foreach (var airport in value.AirportList) JsonSerializer.Serialize(writer, airport, options);
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}