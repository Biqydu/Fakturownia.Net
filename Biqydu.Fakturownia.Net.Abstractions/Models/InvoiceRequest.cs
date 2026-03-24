using System.Text.Json.Serialization;
using Biqydu.Fakturownia.Net.Abstractions.Converters;
using Biqydu.Fakturownia.Net.Abstractions.Models.Constants;
using Biqydu.Fakturownia.Net.Abstractions.Models.Enums;

namespace Biqydu.Fakturownia.Net.Abstractions.Models;

public record InvoiceRequest
{
    [JsonPropertyName("number")]
    public string? Number { get; init; }

    [JsonPropertyName("kind")]
    public string Kind { get; init; } = InvoiceKinds.Vat;

    [JsonPropertyName("sell_date")]
    public required string SellDate { get; init; }

    [JsonPropertyName("issue_date")]
    public required string IssueDate { get; init; }

    [JsonPropertyName("place")]
    public string? Place { get; init; }

    [JsonPropertyName("payment_to")]
    public string? PaymentTo { get; init; }

    [JsonPropertyName("payment_to_kind")]
    public string? PaymentToKind { get; init; }

    [JsonPropertyName("payment_type")]
    public string PaymentType { get; init; } = PaymentMethod.Transfer;

    [JsonPropertyName("currency")]
    public string Currency { get; init; } = Currencies.PLN;

    [JsonPropertyName("lang")]
    public string Lang { get; init; } = Languages.PL;

    [JsonPropertyName("income")]
    [JsonConverter(typeof(IncomeKindConverter))]
    public IncomeKind Income { get; init; } = IncomeKind.Income;

    // -------------------------
    // Seller
    // -------------------------

    /// <summary>
    /// If omitted (along with department_id), the company's default data will be used.
    /// </summary>
    [JsonPropertyName("seller_name")]
    public string? SellerName { get; init; }

    [JsonPropertyName("seller_tax_no")]
    public string? SellerTaxNo { get; init; }

    /// <summary>
    /// Type of seller's tax number. Empty = NIP. Other values: "nip_ue", "other", "empty".
    /// </summary>
    [JsonPropertyName("seller_tax_no_kind")]
    public string? SellerTaxNoKind { get; init; }

    [JsonPropertyName("seller_post_code")]
    public string? SellerPostCode { get; init; }

    [JsonPropertyName("seller_city")]
    public string? SellerCity { get; init; }

    [JsonPropertyName("seller_street")]
    public string? SellerStreet { get; init; }

    [JsonPropertyName("seller_country")]
    public string? SellerCountry { get; init; }

    [JsonPropertyName("seller_email")]
    public string? SellerEmail { get; init; }

    [JsonPropertyName("seller_phone")]
    public string? SellerPhone { get; init; }

    [JsonPropertyName("seller_www")]
    public string? SellerWww { get; init; }

    [JsonPropertyName("seller_fax")]
    public string? SellerFax { get; init; }

    [JsonPropertyName("seller_bank")]
    public string? SellerBank { get; init; }

    [JsonPropertyName("seller_bank_account")]
    public string? SellerBankAccount { get; init; }

    [JsonPropertyName("seller_person")]
    public string? SellerPerson { get; init; }

    // -------------------------
    // Buyer
    // -------------------------

    [JsonPropertyName("buyer_name")]
    public required string BuyerName { get; init; }

    [JsonPropertyName("buyer_tax_no")]
    public string? BuyerTaxNo { get; init; }

    /// <summary>
    /// Type of buyer's tax number. Empty = NIP. Other values: "nip_ue", "other", "empty".
    /// </summary>
    [JsonPropertyName("buyer_tax_no_kind")]
    public string? BuyerTaxNoKind { get; init; }

    [JsonPropertyName("buyer_post_code")]
    public string? BuyerPostCode { get; init; }

    [JsonPropertyName("buyer_city")]
    public string? BuyerCity { get; init; }

    [JsonPropertyName("buyer_street")]
    public string? BuyerStreet { get; init; }

    [JsonPropertyName("buyer_country")]
    public string? BuyerCountry { get; init; }

    [JsonPropertyName("buyer_email")]
    public string? BuyerEmail { get; init; }

    [JsonPropertyName("buyer_note")]
    public string? BuyerNote { get; init; }

    /// <summary>
    /// "1" if the buyer is a company. Empty = private individual.
    /// </summary>
    [JsonPropertyName("buyer_company")]
    public string? BuyerCompany { get; init; }

    [JsonPropertyName("buyer_person")]
    public string? BuyerPerson { get; init; }

    [JsonPropertyName("buyer_first_name")]
    public string? BuyerFirstName { get; init; }

    [JsonPropertyName("buyer_last_name")]
    public string? BuyerLastName { get; init; }

    // -------------------------
    // Department / Client
    // -------------------------

    /// <summary>
    /// Company department ID (Settings > Company Data). If provided, the seller data will be filled with the department's data.
    /// </summary>
    [JsonPropertyName("department_id")]
    public string? DepartmentId { get; init; }

    [JsonPropertyName("client_id")]
    public string? ClientId { get; init; }

    // -------------------------
    // Descriptions
    // -------------------------

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("description_footer")]
    public string? DescriptionFooter { get; init; }

    [JsonPropertyName("description_long")]
    public string? DescriptionLong { get; init; }

    /// <summary>
    /// Private note — not visible on the printed invoice.
    /// </summary>
    [JsonPropertyName("internal_note")]
    public string? InternalNote { get; init; }

    // -------------------------
    // Order / Relations
    // -------------------------

    [JsonPropertyName("oid")]
    public string? Oid { get; init; }

    /// <summary>
    /// Set to "yes" to prevent creating duplicates with the same OID.
    /// </summary>
    [JsonPropertyName("oid_unique")]
    public string? OidUnique { get; init; }

    /// <summary>
    /// ID of the related document (e.g., order for an advance invoice).
    /// </summary>
    [JsonPropertyName("invoice_id")]
    public string? InvoiceId { get; init; }

    /// <summary>
    /// ID of the source invoice (e.g., when generating a VAT invoice from a proforma).
    /// </summary>
    [JsonPropertyName("from_invoice_id")]
    public string? FromInvoiceId { get; init; }

    // -------------------------
    // Discounts
    // -------------------------

    [JsonPropertyName("show_discount")]
    public string ShowDiscount { get; init; } = "0";

    /// <summary>
    /// Discount type: "percent_unit" (percentage of unit net price) or "amount" (fixed amount).
    /// Required when show_discount = "1".
    /// </summary>
    [JsonPropertyName("discount_kind")]
    public string? DiscountKind { get; init; }

    // -------------------------
    // Payments
    // -------------------------

    [JsonPropertyName("split_payment")]
    public string SplitPayment { get; init; } = "0";

    // -------------------------
    // Other
    // -------------------------

    [JsonPropertyName("invoice_template_id")]
    public string? InvoiceTemplateId { get; init; }

    [JsonPropertyName("warehouse_id")]
    public string? WarehouseId { get; init; }

    [JsonPropertyName("additional_info")]
    public string AdditionalInfo { get; init; } = "0";

    /// <summary>
    /// Name of the additional column on invoice positions (e.g. "PKWiU").
    /// </summary>
    [JsonPropertyName("additional_info_desc")]
    public string? AdditionalInfoDesc { get; init; }

    // -------------------------
    // Currency Exchange
    // -------------------------

    /// <summary>
    /// Target currency for conversion (e.g. "PLN").
    /// </summary>
    [JsonPropertyName("exchange_currency")]
    public string? ExchangeCurrency { get; init; }

    /// <summary>
    /// Exchange rate source: "ecb", "nbp", "cbr", "nbu", "nbg", "own".
    /// </summary>
    [JsonPropertyName("exchange_kind")]
    public string? ExchangeKind { get; init; }

    [JsonPropertyName("exchange_currency_rate")]
    public string? ExchangeCurrencyRate { get; init; }

    // -------------------------
    // Positions
    // -------------------------

    [JsonPropertyName("positions")]
    public required List<InvoicePosition> Positions { get; init; } = [];
}