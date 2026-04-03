using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Standard VAT rates supported by Fakturownia.pl.
/// Used in <see cref="InvoicePosition.Tax"/>.
/// </summary>
public static class VatRates
{
    public const string Vat23 = "23";
    public const string Vat8 = "8";
    public const string Vat5 = "5";
    public const string Vat0 = "0";

    /// <summary>
    /// VAT exempt
    /// </summary>
    public const string Exempt = "zw";

    /// <summary>
    ///Not taxable
    /// </summary>
    public const string NotTaxable = "np";

    /// <summary>
    /// Reverse Charge
    /// </summary>
    public const string ReverseCharge = "oo";

    public static decimal? ToRate(string vat)
    {
        if (decimal.TryParse(vat, out var value))
            return value / 100m;

        return vat switch
        {
            Exempt => 0m,
            NotTaxable => 0m,
            ReverseCharge => 0m,
            _ => throw new ArgumentOutOfRangeException(nameof(vat), $"Unknown VAT rate: {vat}")
        };
    }
}