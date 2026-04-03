using Biqydu.Fakturownia.Net.Abstractions.Constants;
using Biqydu.Fakturownia.Net.Abstractions.Enums;

namespace Biqydu.Fakturownia.Net.Abstractions.Models;

public class InvoiceQueryParams
{
    public int? Page { get; set; }
    public int? PerPage { get; set; }

    /// <summary>
    /// Time period (use constants from <see cref="PeriodTemplates"/>)
    /// </summary>
    public string? Period { get; set; }

    /// <summary>
    /// Invoice status (use constants from <see cref="InvoiceStatuses"/>)
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Document type (e.g. vat, proforma, correction, advance...)
    /// </summary>
    public string? Kind { get; set; }

    /// <summary>
    /// Multiple document types (e.g. vat + proforma)
    /// </summary>
    public List<string>? Kinds { get; set; }

    public string? BuyerName { get; set; }
    public string? Oid { get; set; }
    public long? ClientId { get; set; }

    /// <summary>
    /// Invoice number (exact match)
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Filter income (yes) / expense (no)
    /// </summary>
    public IncomeKind? Income { get; set; }

    public long? InvoiceId { get; set; }
    public long? FromInvoiceId { get; set; }

    /// <summary>
    /// Date from (format: YYYY-MM-DD). Works when Period = "more"
    /// </summary>
    public string? DateFrom { get; set; }

    /// <summary>
    /// Date to (format: YYYY-MM-DD). Works when Period = "more"
    /// </summary>
    public string? DateTo { get; set; }

    /// <summary>
    /// Which date field to filter by
    /// </summary>
    public string? SearchDateType { get; set; }

    /// <summary>
    /// Whether to include invoice positions (include_positions=true)
    /// </summary>
    public bool? IncludePositions { get; set; }

    /// <summary>
    /// Result sorting (e.g. issue_date.desc, updated_at, buyer_name, etc.)
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// List of specific invoice IDs to retrieve (invoice_ids=123,456,789)
    /// </summary>
    public List<long>? InvoiceIds { get; set; }

    public string ToQueryString()
    {
        var query = new List<string>();

        if (Page.HasValue) query.Add($"page={Page}");
        if (PerPage.HasValue) query.Add($"per_page={PerPage}");

        if (!string.IsNullOrEmpty(Period)) query.Add($"period={Period}");
        if (!string.IsNullOrEmpty(Status)) query.Add($"status={Status}");
        if (!string.IsNullOrEmpty(Kind)) query.Add($"kind={Kind}");
        if (!string.IsNullOrEmpty(Number)) query.Add($"number={Uri.EscapeDataString(Number)}");
        if (!string.IsNullOrEmpty(BuyerName)) query.Add($"buyer_name={Uri.EscapeDataString(BuyerName)}");
        if (!string.IsNullOrEmpty(Oid)) query.Add($"oid={Uri.EscapeDataString(Oid)}");

        if (ClientId.HasValue) query.Add($"client_id={ClientId}");
        if (InvoiceId.HasValue) query.Add($"invoice_id={InvoiceId}");
        if (FromInvoiceId.HasValue) query.Add($"from_invoice_id={FromInvoiceId}");

        if (Income.HasValue)
            query.Add($"income={(Income == IncomeKind.Income ? "yes" : "no")}");

        if (!string.IsNullOrEmpty(DateFrom)) query.Add($"date_from={DateFrom}");
        if (!string.IsNullOrEmpty(DateTo)) query.Add($"date_to={DateTo}");
        if (!string.IsNullOrEmpty(SearchDateType)) query.Add($"search_date_type={SearchDateType}");

        if (IncludePositions.HasValue)
            query.Add($"include_positions={IncludePositions.Value.ToString().ToLower()}");

        if (!string.IsNullOrEmpty(Order)) query.Add($"order={Order}");

        // invoice_ids=123,456,789
        if (InvoiceIds?.Count > 0)
        {
            var ids = string.Join(",", InvoiceIds);
            query.Add($"invoice_ids={ids}");
        }

        // kinds[]=vat&kinds[]=proforma
        if (Kinds?.Count > 0)
            query.AddRange(Kinds.Select(kind => $"kinds[]={Uri.EscapeDataString(kind)}"));

        return query.Count > 0
            ? "&" + string.Join("&", query)
            : string.Empty;
    }
}