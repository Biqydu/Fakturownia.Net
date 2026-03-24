namespace Biqydu.Fakturownia.Net.Abstractions.Models.Constants;

/// <summary>
/// Languages supported by Fakturownia.pl.
/// </summary>
public static class Languages
{
    public const string PL = "pl";
    public const string EN = "en";
    public const string EN_GB = "en-GB";
    public const string DE = "de";
    public const string FR = "fr";
    public const string CZ = "cz";
    public const string RU = "ru";
    public const string ES = "es";
    public const string IT = "it";
    public const string NL = "nl";
    public const string HR = "hr";
    public const string AR = "ar";
    public const string SK = "sk";
    public const string SL = "sl";
    public const string EL = "el";
    public const string ET = "et";
    public const string CN = "cn";
    public const string HU = "hu";
    public const string TR = "tr";
    public const string FA = "fa";

    /// <summary>
    /// An helper method for creating bilingual invoices.
    /// </summary>
    public static string Bilingual(string lang1, string lang2) => $"{lang1}/{lang2}";
}