using System.Text.Json.Serialization;
using Biqydu.Fakturownia.Net.Abstractions.Converters;

namespace Biqydu.Fakturownia.Net.Abstractions.Models;

public record InvoicePosition
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("tax")]
    public required string Tax { get; init; }

    [JsonPropertyName("quantity")]
    [JsonConverter(typeof(DecimalConverter))]
    public decimal Quantity { get; init; } = 1;

    [JsonPropertyName("quantity_unit")]
    public string QuantityUnit { get; init; } = "szt";

    // -------------------------
    // Prices — provide at least one of the following;
    // the API will calculate the remaining values.
    // -------------------------

    [JsonPropertyName("price_net")]
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? PriceNet { get; init; }

    [JsonPropertyName("price_gross")]
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? PriceGross { get; init; }

    [JsonPropertyName("total_price_net")]
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? TotalPriceNet { get; init; }

    [JsonPropertyName("total_price_gross")]
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? TotalPriceGross { get; init; }

    // -------------------------
    // Discounts
    // -------------------------

    /// <summary>
    /// Percentage discount. Requires show_discount="1" on the invoice
    /// and the setting "How to calculate discount" = "percentage" in Account Settings.
    /// </summary>
    [JsonPropertyName("discount_percent")]
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? DiscountPercent { get; init; }

    /// <summary>
    /// Fixed amount discount. Requires show_discount="1" on the invoice
    /// and the setting "How to calculate discount" = "amount" in Account Settings.
    /// </summary>
    [JsonPropertyName("discount")]
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? Discount { get; init; }

    // -------------------------
    // Identifiers
    // -------------------------

    /// <summary>
    /// Product ID from the Fakturownia system. If provided, the remaining position data
    /// will be automatically filled from the product card.
    /// </summary>
    [JsonPropertyName("product_id")]
    public long? ProductId { get; init; }

    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    /// GTU Code (Goods and Services Groups) — required for JPK_V7.
    /// E.g. "GTU_01", "GTU_12".
    /// </summary>
    [JsonPropertyName("gtu_code")]
    public string? GtuCode { get; init; }

    // -------------------------
    // Descriptions
    // -------------------------

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Additional information in the column (e.g., PKWiU).
    /// Requires additional_info="1" on the invoice.
    /// </summary>
    [JsonPropertyName("additional_info")]
    public string? AdditionalInfo { get; init; }

    // -------------------------
    // Lump-sum tax
    // -------------------------

    /// <summary>
    /// Lump-sum tax rate. Available only when the department/company has enabled the option
    /// "Payer of lump-sum income tax".
    /// </summary>
    [JsonPropertyName("lump_sum_tax")]
    public string? LumpSumTax { get; init; }
}