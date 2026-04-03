using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Advance invoice creation modes in Fakturownia.pl.
/// Used in the <see cref="InvoiceRequest.AdvanceCreationMode"/> field.
/// </summary>
public static class AdvanceCreationModes
{
    /// <summary>
    /// The advance payment is made as a percentage of the order value.
    /// </summary>
    public const string Percent = "percent";

    /// <summary>
    /// The advance payment is created as a specific gross amount.
    /// </summary>
    public const string Amount = "amount";
}