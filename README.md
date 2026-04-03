# Biqydu.Fakturownia.Net

[![NuGet](https://img.shields.io/nuget/v/Biqydu.Fakturownia.Net.DependencyInjection.svg)](https://www.nuget.org/packages/Biqydu.Fakturownia.Net.DependencyInjection)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Biqydu.Fakturownia.Net.DependencyInjection.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/Biqydu.Fakturownia.Net.DependencyInjection)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A clean, fully-typed .NET 8+ client for the Fakturownia.pl invoicing API.

> **Enjoying the SDK?** Drop a ⭐ on GitHub — it really helps others discover it!

## Why this project?

While Fakturownia.pl is popular in Poland, there was no up-to-date, dedicated SDK for modern .NET. I built this to fill that gap with a focus on reliability:

- **Financial precision**: Uses `decimal` for all monetary values to avoid rounding errors (no `double` or `float` here).
- **Resilient by default**: Includes Polly policies to handle transient errors and 429 (rate limit) responses automatically.
- **Security**: Built-in masking for `api_token` in logs to prevent accidental leaks.
- **Native PDF streaming**: Downloads are returned as a Stream, so you can pipe them directly to a file or web response without loading everything into memory.

## Packages

The SDK is split into three parts so you don't have to pull in dependencies you don't need:

- **`Biqydu.Fakturownia.Net.Abstractions`**: Just the models and interfaces. Low footprint, no logic.
- **`Biqydu.Fakturownia.Net`**: The main client implementation.
- **`Biqydu.Fakturownia.Net.DependencyInjection`**: Extensions for `IServiceCollection`. **Start here** if you are using ASP.NET Core.

## Installation

```bash
dotnet add package Biqydu.Fakturownia.Net.DependencyInjection
```

## Configuration

In your `Program.cs`:

```csharp
builder.Services.AddFakturownia(options =>
{
    options.ApiToken = "YOUR_API_TOKEN_HERE";
    options.Subdomain = "your-company"; // e.g. "acme" → acme.fakturownia.pl
});
```

## Basic Usage

### Creating an Invoice

```csharp
const decimal priceNet = 12500.00m;
const int quantity = 3;
var taxRate = VatRates.ToRate(VatRates.Vat23) ?? 0;

var request = new InvoiceRequest
{
    BuyerName = "Global Client Sp. z o.o.",
    BuyerTaxNo = "PL5250001090",
    Currency = Currencies.EUR,
    Lang = Languages.EN,
    SellDate = DateTime.Today.ToString("yyyy-MM-dd"),
    IssueDate = DateTime.Today.ToString("yyyy-MM-dd"),
    Positions =
    [
        new InvoicePosition
        {
            Name = "Backend development services - March 2026",
            Tax = "23",
            Quantity = quantity,
            TotalPriceGross = (priceNet * quantity) * (1 + taxRate),
            QuantityUnit = "service"
        }
    ]
};

var invoice = await fakturowniaClient.CreateInvoiceAsync(request);
Console.WriteLine($"Invoice created: {invoice.Number}");
```

### Downloading the PDF

```csharp
await using var pdfStream = await fakturowniaClient.GetInvoicePdfAsync(invoice.Id);
await using var fileStream = File.Create($"FV_{invoice.Number.Replace("/", "-")}.pdf");
await pdfStream.CopyToAsync(fileStream);
```

### Sending by Email

```csharp
await fakturowniaClient.SendByEmailAsync(invoice.Id);
```

## Advanced Examples

### Adding Discounts and GTU Codes

```csharp
const decimal priceNet = 12500.00m;
const int quantity = 3;
const decimal discountPercent = 5m;
// Helper to get decimal rate (e.g., 0.23) from string constant
var taxRate = VatRates.ToRate(VatRates.Vat23) ?? 0;

// 1. Calculate unit price after discount
var priceAfterDiscount = Math.Round(priceNet * (1 - (discountPercent / 100)), 2);
// 2. Calculate total net for all items
var totalNet = Math.Round(priceAfterDiscount * quantity, 2);
// 3. Calculate final gross amount
var totalGross = Math.Round(totalNet * (1 + taxRate), 2);

var request = new InvoiceRequest
{
    BuyerName = "Global Client Sp. z o.o.",
    BuyerTaxNo = "PL5250001090",
    Currency = Currencies.EUR,
    Lang = Languages.EN,
    ShowDiscount = LogicalStatus.Yes,
    DiscountKind = DiscountKinds.PercentUnit,
    SellDate = DateTime.Today.ToString("yyyy-MM-dd"),
    IssueDate = DateTime.Today.ToString("yyyy-MM-dd"),
    Positions = [
        new InvoicePosition
        {
            Name = "Dell XPS 15 Laptop",
            Tax = VatRates.Vat23,
            PriceNet = priceNet,         // Unit price BEFORE discount
            Quantity = quantity,
            GtuCode = GtuCodes.Gtu06,    // Required for some electronics in Poland
            DiscountPercent = discountPercent,
            TotalPriceGross = totalGross // Manually calculated to avoid rounding issues
        }
    ]
};

var response = await client.CreateInvoiceAsync(request);
```

### Currency Exchange

```csharp
request.ExchangeCurrency = Currencies.PLN;
request.ExchangeKind = "nbp";
```

### Lump-sum Tax

```csharp
position.LumpSumTax = "8.5"; // only if your company uses lump-sum taxation
```

## Error Handling

```csharp
try
{
    var invoice = await client.CreateInvoiceAsync(request);
}
catch (FakturowniaException ex)
{
    Console.WriteLine($"HTTP Status: {ex.StatusCode}");
    Console.WriteLine($"Fakturownia response: {ex.ResponseBody}");
    // ex contains validation errors, rate limits, auth issues, etc.
}
```

## Logging & Debugging

The SDK integrates with `Microsoft.Extensions.Logging` and is optional but very useful.

Just enable console (or any other) logging in your app:

```csharp
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
```

The `api_token` is automatically masked in logs for security.

## Documentation

- [Advanced Examples & Rounding Guide](README.md#advanced-examples)
- [Full Constants & Enums Reference](CONSTANTS.md)

## Contributing

Contributions are welcome!

1. Fork the project
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

Distributed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

**Note**: This is an unofficial .NET SDK for Fakturownia.pl and is not affiliated with or supported by them.

Maintained by Biqydu. Bug reports and PRs are welcome.
