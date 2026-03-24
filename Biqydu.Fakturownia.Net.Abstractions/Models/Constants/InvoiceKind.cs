namespace Biqydu.Fakturownia.Net.Abstractions.Models.Constants;

/// <summary>
/// Document types supported by Fakturownia.pl.
/// </summary>
public static class InvoiceKinds
{
    public const string Vat = "vat";
    public const string Proforma = "proforma";
    public const string Bill = "bill";
    public const string Receipt = "receipt";
    public const string Advance = "advance";
    public const string Correction = "correction";
    public const string VatMp = "vat_mp";
    public const string InvoiceOther = "invoice_other";
    public const string VatMargin = "vat_margin";
    public const string Kp = "kp";
    public const string Kw = "kw";
    public const string Final = "final";
    public const string Estimate = "estimate";
}