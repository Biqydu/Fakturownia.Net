using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Common currency codes supported by Fakturownia.pl.
/// Use these constants for <see cref="InvoiceRequest.Currency"/>.
/// </summary>
public static class Currencies
{
    public const string PLN = "PLN";
    public const string EUR = "EUR";
    public const string USD = "USD";
    public const string GBP = "GBP";
    public const string CHF = "CHF";
}