using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// ISO 3166-1 alpha-2 country codes supported by Fakturownia.pl.
/// Used in <see cref="InvoiceRequest.SellerCountry"/> and <see cref="InvoiceRequest.BuyerCountry"/>.
/// </summary>
public static class Countries
{
    public const string Poland = "PL";
    public const string Germany = "DE";
    public const string France = "FR";
    public const string Italy = "IT";
    public const string Spain = "ES";
    public const string Netherlands = "NL";
    public const string Belgium = "BE";
    public const string Czechia = "CZ";
    public const string Slovakia = "SK";
    public const string Austria = "AT";
    public const string Ireland = "IE";
    public const string Denmark = "DK";
    public const string Sweden = "SE";
    public const string Finland = "FI";
    public const string Portugal = "PT";
    public const string Greece = "GR";
    public const string Hungary = "HU";
    public const string Lithuania = "LT";
    public const string Latvia = "LV";
    public const string Estonia = "EE";
    public const string Romania = "RO";
    public const string Bulgaria = "BG";
    public const string Croatia = "HR";
    public const string Slovenia = "SI";
    public const string Cyprus = "CY";
    public const string Malta = "MT";

    public const string UnitedKingdom = "GB";
    public const string UnitedStates = "US";
    public const string Switzerland = "CH";
    public const string Norway = "NO";
    public const string Ukraine = "UA";
    public const string China = "CN";
    public const string Japan = "JP";
}