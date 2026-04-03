using System.Text.Json;
using System.Text.Json.Serialization;
using Biqydu.Fakturownia.Net.Abstractions.Enums;

namespace Biqydu.Fakturownia.Net.Abstractions.Converters;

public class LogicalStatusConverter : JsonConverter<LogicalStatus>
{
    public override bool HandleNull => true;

    public override LogicalStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return LogicalStatus.No;

            case JsonTokenType.Number:
                return reader.GetInt32() == 1 ? LogicalStatus.Yes : LogicalStatus.No;

            case JsonTokenType.String:
                var value = reader.GetString();
                return value == "1" ? LogicalStatus.Yes : LogicalStatus.No;

            default:
                return LogicalStatus.No;
        }
    }

    public override void Write(Utf8JsonWriter writer, LogicalStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value == LogicalStatus.Yes ? "1" : "0");
    }
}