using Biqydu.Fakturownia.Net.Abstractions.Models.Enums;

namespace Biqydu.Fakturownia.Net.Abstractions.Models;

public class InvoiceQueryParams
{
    public int? Page { get; set; }
    public int? PerPage { get; set; }
    public string? Period { get; set; }
    public string? Status { get; set; }
    public string? BuyerName { get; set; }
    public string? Oid { get; set; }

    /// <summary>
    /// Filter by document type: Income or Expense.
    /// </summary>
    public IncomeKind? Income { get; set; }

    /// <summary>
    /// Filter invoices related to a document with a given ID.
    /// </summary>
    public long? InvoiceId { get; set; }

    /// <summary>
    /// Filter invoices generated based on a document with a given ID (e.g. VAT from Proforma).
    /// </summary>
    public long? FromInvoiceId { get; set; }

    public string ToQueryString()
    {
        var paramsList = new List<string>();

        if (Page.HasValue) paramsList.Add($"page={Page}");
        if (PerPage.HasValue) paramsList.Add($"per_page={PerPage}");
        if (!string.IsNullOrEmpty(Period)) paramsList.Add($"period={Period}");
        if (!string.IsNullOrEmpty(Status)) paramsList.Add($"status={Status}");
        if (!string.IsNullOrEmpty(BuyerName)) paramsList.Add($"buyer_name={Uri.EscapeDataString(BuyerName)}");
        if (!string.IsNullOrEmpty(Oid)) paramsList.Add($"oid={Uri.EscapeDataString(Oid)}");
        if (Income.HasValue) paramsList.Add($"income={(Income == IncomeKind.Income ? "yes" : "no")}");
        if (InvoiceId.HasValue) paramsList.Add($"invoice_id={InvoiceId}");
        if (FromInvoiceId.HasValue) paramsList.Add($"from_invoice_id={FromInvoiceId}");

        return paramsList.Count > 0 ? "&" + string.Join("&", paramsList) : "";
    }
}