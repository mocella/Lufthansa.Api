using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Reference;

namespace Lufthansa.Api.Models.JsonConverters;

public class CitiesConverter : JsonConverter<Cities>
{
    public override Cities Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var cities = new Cities();

        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start of object");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName!.Equals("City", StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // Single city case
                        var city = JsonSerializer.Deserialize<City>(ref reader, options);
                        if (city != null) cities.CityList!.Add(city);
                    }
                    else if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        // Array of cities case
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            if (reader.TokenType == JsonTokenType.StartObject)
                            {
                                var city = JsonSerializer.Deserialize<City>(ref reader, options);
                                if (city != null) cities.CityList!.Add(city);
                            }
                    }
                }
                else
                {
                    // Skip other properties
                    reader.Skip();
                }
            }

        return cities;
    }

    public override void Write(Utf8JsonWriter writer, Cities value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("City");
        if (value.CityList!.Count == 1)
        {
            // Single city case
            JsonSerializer.Serialize(writer, value.CityList[0], options);
        }
        else
        {
            // Array of cities case
            writer.WriteStartArray();
            foreach (var city in value.CityList) JsonSerializer.Serialize(writer, city, options);
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}