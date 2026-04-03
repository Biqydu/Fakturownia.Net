using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Predefined period templates for filtering document lists.
/// These values can be used in <see cref="InvoiceQueryParams.Period"/>.
/// </summary>
public static class PeriodTemplates
{
    public const string ThisMonth = "this_month";
    public const string LastMonth = "last_month";
    public const string Last30Days = "last_30_days";
    public const string ThisYear = "this_year";
    public const string LastYear = "last_year";
    public const string All = "all";

    /// <summary>Requires additional date_from and date_to parameters.</summary>
    public const string More = "more";
}