namespace Biqydu.Fakturownia.Net.Abstractions.Models.Constants;

/// <summary>
/// Predefined payment methods supported by Fakturownia.pl.
/// You can use these constants or provide your own text string.
/// </summary>
public static class PaymentMethod
{
    public const string Transfer = "transfer";
    public const string Card = "card";
    public const string Cash = "cash";
    public const string Barter = "barter";
    public const string Cheque = "cheque";
    public const string BillOfExchange = "bill_of_exchange";
    public const string CashOnDelivery = "cash_on_delivery";
    public const string Compensation = "compensation";
    public const string LetterOfCredit = "letter_of_credit";
    public const string PayU = "payu";
    public const string PayPal = "paypal";
    public const string Off = "off";
}