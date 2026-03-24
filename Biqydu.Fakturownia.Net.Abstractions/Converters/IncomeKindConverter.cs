using System.Text.Json;
using System.Text.Json.Serialization;
using Biqydu.Fakturownia.Net.Abstractions.Models.Enums;

namespace Biqydu.Fakturownia.Net.Abstractions.Converters;

public class IncomeKindConverter : JsonConverter<IncomeKind>
{
    public override IncomeKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value == "0" ? IncomeKind.Expense : IncomeKind.Income;
    }

    public override void Write(Utf8JsonWriter writer, IncomeKind value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((int)value).ToString());
    }
}