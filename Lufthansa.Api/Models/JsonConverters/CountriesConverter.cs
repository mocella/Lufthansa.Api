using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Reference;

namespace Lufthansa.Api.Models.JsonConverters;

public class CountriesConverter : JsonConverter<Countries>
{
    public override Countries Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var countries = new Countries();

        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start of object");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName!.Equals("Country", StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // Single country case
                        var country = JsonSerializer.Deserialize<Country>(ref reader, options);
                        if (country != null) countries.CountryList!.Add(country);
                    }
                    else if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        // Array of countries case
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            if (reader.TokenType == JsonTokenType.StartObject)
                            {
                                var country = JsonSerializer.Deserialize<Country>(ref reader, options);
                                if (country != null) countries.CountryList!.Add(country);
                            }
                    }
                }
                else
                {
                    // Skip other properties
                    reader.Skip();
                }
            }

        return countries;
    }

    public override void Write(Utf8JsonWriter writer, Countries value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Country");
        if (value.CountryList!.Count == 1)
        {
            // Single country case
            JsonSerializer.Serialize(writer, value.CountryList[0], options);
        }
        else
        {
            // Array of countries case
            writer.WriteStartArray();
            foreach (var country in value.CountryList) JsonSerializer.Serialize(writer, country, options);
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}