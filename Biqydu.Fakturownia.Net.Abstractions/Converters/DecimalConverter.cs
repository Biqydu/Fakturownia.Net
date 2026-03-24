using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Biqydu.Fakturownia.Net.Abstractions.Converters;

/// <summary>
/// The Invoice API returns decimal values both as numbers (e.g., 13726.8)
/// and as strings with commas (e.g., "13726.80"). This converter supports both cases.
/// </summary>
public class DecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return reader.GetDecimal();
            case JsonTokenType.String:
            {
                var str = reader.GetString();
                return string.IsNullOrWhiteSpace(str) ? 0 : decimal.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);
            }
            default:
                throw new JsonException($"Unexpected token type {reader.TokenType} for decimal.");
        }
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}

/// <summary>
/// Nullable version for optional fields.
/// </summary>
public class NullableDecimalConverter : JsonConverter<decimal?>
{
    public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.Number:
                return reader.GetDecimal();
            case JsonTokenType.String:
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str)) return null;

                return decimal.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);
            }
            default:
                throw new JsonException($"Unexpected token type {reader.TokenType} for decimal?.");
        }
    }

    public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
    {
        if (value is null) writer.WriteNullValue();
        else writer.WriteNumberValue(value.Value);
    }
}