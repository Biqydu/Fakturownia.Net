using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Date types used for filtering and searching documents.
/// Use these in <see cref="InvoiceQueryParams.SearchDateType"/>.
/// </summary>
public static class SearchDateTypes
{
    public const string IssueDate = "issue_date";
    public const string PaidDate = "paid_date";
    public const string TransactionDate = "transaction_date";
}