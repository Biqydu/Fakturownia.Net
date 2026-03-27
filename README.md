# Biqydu.Fakturownia.Net

[![NuGet](https://img.shields.io/nuget/v/Biqydu.Fakturownia.Net.DependencyInjection.svg)](https://www.nuget.org/packages/Biqydu.Fakturownia.Net.DependencyInjection)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Biqydu.Fakturownia.Net.DependencyInjection.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/Biqydu.Fakturownia.Net.DependencyInjection)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A modern, high-performance, strictly typed .NET 8+ SDK for the [Fakturownia.pl](https://fakturownia.pl) invoicing API.
> **Enjoying this SDK?** Please consider giving it a ⭐ **Star** on GitHub to help others find it!

Built with `IHttpClientFactory`, `System.Text.Json`, and `decimal` for full financial precision.

## 🚀 Key Features

- Full asynchronous, typed client using `HttpClientFactory`
- Complete support for creating and managing invoices
- Accurate financial calculations using `decimal`
- Efficient PDF handling as `Stream`
- Built-in support for email sending and status changes
- Excellent Dependency Injection support for ASP.NET Core
- Comprehensive models with full XML documentation
- **Built-in Resilience**: Includes automatic retries with exponential backoff (via Polly) for transient errors and rate limiting (429).
- **Secure Logging**: Integrated `ILogger` support with automatic `api_token` masking.

## 📦 Packages

| Package                                      | Purpose                                      | Recommended |
|---------------------------------------------|----------------------------------------------|-------------|
| `Biqydu.Fakturownia.Net.Abstractions`       | Models, enums, DTOs and interfaces           | Yes         |
| `Biqydu.Fakturownia.Net`                    | Core API client implementation               | Yes         |
| `Biqydu.Fakturownia.Net.DependencyInjection`| `IServiceCollection` extensions              | **Yes**     |

## 🛠️ Installation

```bash
dotnet add package Biqydu.Fakturownia.Net.DependencyInjection
```

## ⚙️ Configuration

In `Program.cs`:

```csharp
builder.Services.AddFakturownia(options =>
{
    options.ApiToken = "YOUR_API_TOKEN_HERE";
    options.Subdomain = "your-company"; // np. "acme" → acme.fakturownia.pl
});
```

## 📖 Basic Usage

### Creating an Invoice

```csharp
const decimal priceNet = 12500.00m;
const int quantity = 3;
const decimal taxRate = 0.23m; 

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

### Downloading PDF

```csharp
await using var pdfStream = await fakturowniaClient.GetInvoicePdfAsync(invoice.Id);

await using var fileStream = File.Create($"FV_{invoice.Number.Replace("/", "-")}.pdf");
await pdfStream.CopyToAsync(fileStream);
```

### Sending by Email

```csharp
await fakturowniaClient.SendByEmailAsync(invoice.Id);
```

## ✨ Advanced Usage

### Discounts + GTU Code (JPK_V7)

```csharp
var position = new InvoicePosition
{
    Name = "Laptop Dell XPS 15",
    Tax = "23",
    PriceNet = 5200m,
    Quantity = 2,
    GtuCode = "GTU_01",      
    DiscountPercent = 5m          
};
```

### Currency Exchange

```csharp
request.ExchangeCurrency = Currencies.PLN;
request.ExchangeKind = "nbp";  
```

### Lump-sum Tax

```csharp
position.LumpSumTax = "8.5";      // only when company has lump-sum tax enabled
```

## ⚠️ Error Handling

```csharp
try
{
    var invoice = await client.CreateInvoiceAsync(request);
}
catch (FakturowniaException ex)
{
    Console.WriteLine($"HTTP Status: {ex.StatusCode}");
    Console.WriteLine($"Fakturownia response: {ex.ResponseBody}");
    // ex contains validation errors, rate limits, authorization issues, etc.
}
```

## 🪵 Logging & Debugging

The SDK uses the standard `Microsoft.Extensions.Logging` abstractions. It is **optional** and **secure** by design.

### Why use it?
Fakturownia API can be unpredictable. Logging allows you to see the exact reason for failures (like validation errors or 422 Unprocessable Entity) that are otherwise hidden.

### Enabling Logs
If you are using Dependency Injection, just configure your logging provider:

```csharp
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug); // To see detailed SDK actions
```

## 🤝 Contributing

Contributions are welcome!

1. Fork the project
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📜 License

Distributed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

**Disclaimer**: This is an **unofficial** SDK and is not affiliated with, endorsed by, or supported by Fakturownia.pl.

Made with ❤️ for the .NET community.
