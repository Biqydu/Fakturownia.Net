using Biqydu.Fakturownia.Net.Abstractions;
using Biqydu.Fakturownia.Net.Abstractions.Models;
using Biqydu.Fakturownia.Net.Abstractions.Models.Constants;
using Biqydu.Fakturownia.Net.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddFakturownia(options =>
{
    options.ApiToken = "NNOLeuh6Fpgsy8cOT8Jq";
    options.Subdomain = "biqydu";
});

using var host = builder.Build();
var client = host.Services.GetRequiredService<IFakturowniaClient>();

var todayDate = DateTime.Now;
var today = todayDate.ToString("yyyy-MM-dd");
var paymentDue = todayDate.AddDays(14).ToString("yyyy-MM-dd");

var invoice = new InvoiceRequest
{
    SellDate = today,
    IssueDate = today,
    PaymentTo = paymentDue,

    SellerName = "Moja Firma Programistyczna",
    SellerTaxNo = "1140052546",
    SellerCity = "Warszawa",
    SellerStreet = "ul. Programistów 10",
    SellerCountry = "PL",
    SellerEmail = "kontakt@mojafirma.pl",
    SellerBank = "PKO BP",
    SellerBankAccount = "12 3456 7890 1234 5678 9012 3456",
    SellerPerson = "Jan Kowalski",

    BuyerName = "Klient Testowy Sp. z o.o.",
    BuyerTaxNo = "5250001090",
    BuyerCity = "Kraków",
    BuyerStreet = "ul. Testowa 5",
    BuyerCountry = "PL",
    BuyerEmail = "biuro@klienttestowy.pl",
    BuyerPerson = "Anna Nowak",

    Description = "Faktura za usługi programistyczne w marcu 2026",
    DescriptionFooter = "Dziękujemy za współpracę!",

    Place = "Warszawa",

    Positions =
    [
        new InvoicePosition
        {
            Name = "Usługa Programistyczna – Backend",
            Tax = "23",
            PriceNet = 180m,
            Quantity = 62,
            TotalPriceGross = 180m * 62 * 1.23m,
            QuantityUnit = "h",
            Description = "Tworzenie i utrzymanie API",
            AdditionalInfo = "Zaliczkowa płatność 50%"
        },
        new InvoicePosition
        {
            Name = "Usługa Programistyczna – Frontend",
            Tax = "23",
            PriceNet = 150m,
            Quantity = 35,
            TotalPriceGross = 150m * 35 * 1.23m,
            QuantityUnit = "h",
            Description = "Tworzenie interfejsu użytkownika",
        }
    ]
};

var query = new InvoiceQueryParams
{
    Period = InvoicePeriod.All,
    Status = InvoiceStatus.Issued,
};

try
{
    Console.WriteLine("Wysyłanie faktury...");
    var result = await client.CreateInvoiceAsync(invoice);
    Console.WriteLine($"Faktura utworzona: #{result.Number} (ID: {result.Id})");

    Console.WriteLine("Pobieranie faktury...");
    var invoiceResult = await client.GetInvoiceAsync(result.Id);
    Console.WriteLine($"  Numer:       {invoiceResult.Number}");
    Console.WriteLine($"  Status:      {invoiceResult.Status}");
    Console.WriteLine($"  Brutto:      {invoiceResult.TotalPriceGross:F2} {invoiceResult.Currency}");
    Console.WriteLine($"  Netto:       {invoiceResult.TotalPriceNet:F2} {invoiceResult.Currency}");
    Console.WriteLine($"  VAT:         {invoiceResult.TotalPriceTax:F2} {invoiceResult.Currency}");
    Console.WriteLine($"  Zapłacono:   {invoiceResult.Paid:F2} {invoiceResult.Currency}");
    Console.WriteLine($"  Do zapłaty:  {invoiceResult.ToPay:F2} {invoiceResult.Currency}");
    Console.WriteLine($"  Link:        {invoiceResult.ViewUrl}");
    Console.WriteLine("  Pozycje:");
    foreach (var pos in invoiceResult.Positions ?? [])
        Console.WriteLine($"    - {pos.Name}: {pos.TotalPriceGross:F2} brutto (qty: {pos.Quantity} {pos.QuantityUnit})");

    Console.WriteLine("\nPobieranie PDF...");
    await using var pdfStream = await client.GetInvoicePdfAsync(result.Id);
    await using var fileStream = File.Create($"faktura_{result.Id}.pdf");
    await pdfStream.CopyToAsync(fileStream);
    Console.WriteLine($"PDF zapisany: faktura_{result.Id}.pdf");

    Console.WriteLine("\nLista faktur (wszystkie, status: wystawione):");
    var invoices = await client.GetInvoicesAsync(query);
    foreach (var inv in invoices)
        Console.WriteLine($"  {inv.Number,-25} | {inv.ToPay,10:F2} {inv.Currency} | {inv.BuyerName}");

    await client.SendByEmailAsync(result.Id);

    Console.WriteLine($"\nCzy usunąć fakturę {result.Id}? (y/n)");
    if (Console.ReadLine()?.ToLower() == "y")
    {
        await client.DeleteInvoiceAsync(result.Id);
        Console.WriteLine("Faktura została usunięta.");
    }
}
catch (FakturowniaException ex)
{
    Console.WriteLine($"Błąd API ({ex.StatusCode}): {ex.Message}");
    Console.WriteLine($"Response body: {ex.ResponseBody}");
}