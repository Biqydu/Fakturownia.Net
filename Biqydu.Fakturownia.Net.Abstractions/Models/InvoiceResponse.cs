using System.Text.Json.Serialization;
using Biqydu.Fakturownia.Net.Abstractions.Converters;

namespace Biqydu.Fakturownia.Net.Abstractions.Models;

public record InvoiceResponse
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("number")]
    public string? Number { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("kind")]
    public string? Kind { get; init; }

    [JsonPropertyName("price_gross")]
    [JsonConverter(typeof(DecimalConverter))]
    public decimal TotalPriceGross { get; init; }

    [JsonPropertyName("price_net")]
    [JsonConverter(typeof(DecimalConverter))]
    public decimal TotalPriceNet { get; init; }

    [JsonPropertyName("price_tax")]
    [JsonConverter(typeof(DecimalConverter))]
    public decimal TotalPriceTax { get; init; }

    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    [JsonPropertyName("paid")]
    [JsonConverter(typeof(DecimalConverter))]
    public decimal Paid { get; init; }

    /// <summary>
    ///Amount to be paid. The API does not return this field - calculated as TotalPriceGross - Paid.
    /// </summary>
    [JsonIgnore]
    public decimal ToPay => TotalPriceGross - Paid;

    [JsonPropertyName("issue_date")]
    public string? IssueDate { get; init; }

    [JsonPropertyName("sell_date")]
    public string? SellDate { get; init; }

    [JsonPropertyName("payment_to")]
    public string? PaymentTo { get; init; }

    [JsonPropertyName("payment_type")]
    public string? PaymentType { get; init; }

    [JsonPropertyName("seller_name")]
    public string? SellerName { get; init; }

    [JsonPropertyName("buyer_name")]
    public string? BuyerName { get; init; }

    [JsonPropertyName("buyer_tax_no")]
    public string? BuyerTaxNo { get; init; }

    [JsonPropertyName("view_url")]
    public string? ViewUrl { get; init; }

    [JsonPropertyName("oid")]
    public string? Oid { get; init; }

    [JsonPropertyName("client_id")]
    public long? ClientId { get; init; }

    [JsonPropertyName("department_id")]
    public long? DepartmentId { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("internal_note")]
    public string? InternalNote { get; init; }

    [JsonPropertyName("income")]
    public bool Income { get; init; }

    [JsonPropertyName("positions")]
    public List<InvoicePosition>? Positions { get; init; }
}