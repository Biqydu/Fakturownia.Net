using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Official GTU codes (Goods and Services Groups) required in JPK_V7 structure.
/// These values can be used in <see cref="InvoicePosition.GtuCode"/>.
/// </summary>
public static class GtuCodes
{
    /// <summary>Alcoholic beverages (ethyl alcohol, beer, wine, etc.)</summary>
    public const string Gtu01 = "GTU_01";

    /// <summary>Goods referred to in Article 103, paragraph 5aa of the Act (including fuels)</summary>
    public const string Gtu02 = "GTU_02";

    /// <summary>Heating oil and lubricating oils</summary>
    public const string Gtu03 = "GTU_03";

    /// <summary>Tobacco products, dried tobacco, e-cigarette liquid</summary>
    public const string Gtu04 = "GTU_04";

    /// <summary>Waste (only those specified in items 79-91 of Annex No. 15 to the Act)</summary>
    public const string Gtu05 = "GTU_05";

    /// <summary>Electronic devices and parts thereof (selected items from Annex 15)</summary>
    public const string Gtu06 = "GTU_06";

    /// <summary>Vehicles and vehicle parts (CN 8701 - 8708)</summary>
    public const string Gtu07 = "GTU_07";

    /// <summary>Precious and base metals</summary>
    public const string Gtu08 = "GTU_08";

    /// <summary>Medicines and medical devices subject to mandatory notification</summary>
    public const string Gtu09 = "GTU_09";

    /// <summary>Buildings, structures and land</summary>
    public const string Gtu10 = "GTU_10";

    /// <summary>Services for the transfer of greenhouse gas emission allowances</summary>
    public const string Gtu11 = "GTU_11";

    /// <summary>Intangible services (consulting, accounting, legal, management, marketing, etc.)</summary>
    public const string Gtu12 = "GTU_12";

    /// <summary>Transport and warehouse management services</summary>
    public const string Gtu13 = "GTU_13";
}