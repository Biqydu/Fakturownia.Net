using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Standard invoice statuses in Fakturownia.pl.
/// These constants can be used in <see cref="InvoiceRequest.Status"/> 
/// and as a filter in <see cref="InvoiceQueryParams.Status"/>.
/// </summary>
public static class InvoiceStatuses
{
    public const string Issued = "issued";
    public const string Sent = "sent";
    public const string Paid = "paid";
    public const string Partial = "partial";
    public const string Rejected = "rejected";
    public const string Cancelled = "cancelled";
}