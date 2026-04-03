using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Types of discounts supported by Fakturownia.pl.
/// Used in <see cref="InvoiceRequest.DiscountKind"/>.
/// </summary>
public static class DiscountKinds
{
    /// <summary>
    /// Percentage discount calculated from the net unit price.
    /// </summary>
    public const string PercentUnit = "percent_unit";

    /// <summary>
    /// Percentage discount calculated from the gross unit price.
    /// </summary>
    public const string PercentUnitGross = "percent_unit_gross";

    /// <summary>
    /// Percentage discount calculated from the total price of the item.
    /// </summary>
    public const string PercentTotal = "percent_total";

    /// <summary>
    /// Amount discount - a fixed value subtracted from the price.
    /// </summary>
    public const string Amount = "amount";
}