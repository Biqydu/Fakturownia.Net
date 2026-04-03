using System.Text.Json.Serialization;
using Biqydu.Fakturownia.Net.Abstractions.Constants;
using Biqydu.Fakturownia.Net.Abstractions.Converters;
using Biqydu.Fakturownia.Net.Abstractions.Enums;

namespace Biqydu.Fakturownia.Net.Abstractions.Models;

public record InvoiceRequest
{
    [JsonPropertyName("number")]
    public string? Number { get; init; }

    /// <summary>
    /// Type of the document. 
    /// You can use constants from <see cref="InvoiceKinds"/> or provide a custom string.
    /// </summary>
    [JsonPropertyName("kind")]
    public string Kind { get; init; } = InvoiceKinds.Vat;

    /// <summary>
    /// Invoice status (e.g., issued, paid). 
    /// You can use constants from <see cref="InvoiceStatuses"/> or provide a custom string.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = InvoiceStatuses.Issued;

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

    /// <summary>
    /// Payment method. Use constants from <see cref="PaymentMethods"/>.
    /// </summary>
    [JsonPropertyName("payment_type")]
    public string PaymentType { get; init; } = PaymentMethods.Transfer;

    /// <summary>
    /// Currency code (e.g., PLN, EUR, USD). 
    /// You can use constants from <see cref="Currencies"/> or provide any valid ISO currency code as a string.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = Currencies.PLN;

    /// <summary>
    /// Document language (e.g., "pl", "en", "de").
    /// You can use constants from <see cref="Languages"/>.
    /// </summary>
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

    /// <summary>
    /// Represents the tax identification number of the seller.
    /// </summary>
    [JsonPropertyName("seller_tax_no")]
    public string? SellerTaxNo { get; init; }

    /// <summary>
    /// Type of seller's tax number. 
    /// You can use constants from <see cref="TaxNoKinds"/>.
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

    /// <summary>
    /// Represents the tax identification number of the buyer.
    /// </summary>
    [JsonPropertyName("buyer_tax_no")]
    public string? BuyerTaxNo { get; init; }

    /// <summary>
    /// Type of buyer's tax number. 
    /// You can use constants from <see cref="TaxNoKinds"/>.
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

    [JsonPropertyName("buyer_company")]
    public bool? BuyerCompany { get; init; }

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

    [JsonPropertyName("use_oss")]
    [JsonConverter(typeof(LogicalStatusConverter))]
    public LogicalStatus? UseOss { get; init; }

    [JsonPropertyName("fill_default_descriptions")]
    public bool? FillDefaultDescriptions { get; init; }

    [JsonPropertyName("use_prices_from_price_lists")]
    public bool? UsePricesFromPriceLists { get; init; }

    [JsonPropertyName("price_list_id")]
    public string? PriceListId { get; init; }

    [JsonPropertyName("buyer_override")]
    public bool? BuyerOverride { get; init; }

    [JsonPropertyName("correction_reason")]
    public string? CorrectionReason { get; init; }

    /// <summary>
    /// Advance invoice creation mode.
    /// You can use constants from <see cref="AdvanceCreationModes"/>.
    /// Only works when Kind = "advance" and copy_invoice_from (order ID) is provided.
    /// </summary>
    [JsonPropertyName("advance_creation_mode")]
    public string? AdvanceCreationMode { get; init; }

    [JsonPropertyName("advance_value")]
    public string? AdvanceValue { get; init; }

    [JsonPropertyName("position_name")]
    public string? PositionName { get; init; }

    // -------------------------
    // Discounts
    // -------------------------

    /// <summary>
    /// Whether to show the discount column on the invoice.
    /// </summary>
    [JsonPropertyName("show_discount")]
    [JsonConverter(typeof(LogicalStatusConverter))]
    public LogicalStatus ShowDiscount { get; init; } = LogicalStatus.No;

    /// <summary>
    /// Discount type. Required when show_discount = LogicalStatus.Yes.
    /// You can use constants from <see cref="DiscountKinds"/>.
    /// </summary>
    [JsonPropertyName("discount_kind")]
    public string? DiscountKind { get; init; }

    // -------------------------
    // Payments
    // -------------------------

    [JsonPropertyName("split_payment")]
    [JsonConverter(typeof(LogicalStatusConverter))]
    public LogicalStatus SplitPayment { get; init; } = LogicalStatus.No;

    // -------------------------
    // Other
    // -------------------------

    [JsonPropertyName("invoice_template_id")]
    public string? InvoiceTemplateId { get; init; }

    [JsonPropertyName("warehouse_id")]
    public string? WarehouseId { get; init; }

    /// <summary>
    /// Whether to show the additional info column (e.g., PKWiU).
    /// </summary>
    [JsonPropertyName("additional_info")]
    [JsonConverter(typeof(LogicalStatusConverter))]
    public LogicalStatus AdditionalInfo { get; init; } = LogicalStatus.No;

    /// <summary>
    /// Name of the additional column on invoice positions (e.g. "PKWiU").
    /// </summary>
    [JsonPropertyName("additional_info_desc")]
    public string? AdditionalInfoDesc { get; init; }

    [JsonPropertyName("gov_save_and_send")]
    public bool? GovSaveAndSend { get; init; }

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


    [JsonPropertyName("positions")]
    public required List<InvoicePosition> Positions { get; init; } = [];

    [JsonPropertyName("procedure_designations")]
    public List<string>? ProcedureDesignations { get; init; }
}