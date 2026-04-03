using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Biqydu.Fakturownia.Net.Abstractions;
using Biqydu.Fakturownia.Net.Abstractions.Constants;
using Biqydu.Fakturownia.Net.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Biqydu.Fakturownia.Net;

public class FakturowniaClient(
    HttpClient httpClient,
    IOptions<FakturowniaOptions> options,
    [FromKeyedServices("FakturowniaJsonOptions")] JsonSerializerOptions jsonSerializerOptions,
    ILogger<FakturowniaClient>? logger = null)
    : IFakturowniaClient
{
    private readonly FakturowniaOptions _options = options.Value;
    private readonly ILogger<FakturowniaClient> _logger = logger ?? NullLogger<FakturowniaClient>.Instance;

    public async Task<InvoiceResponse> CreateInvoiceAsync(InvoiceRequest request, CancellationToken ct = default)
    {
        LogAction("Creating a new invoice", "POST", "invoices.json");

        var payload = new { api_token = _options.ApiToken, invoice = request };
        var response = await httpClient.PostAsJsonAsync("invoices.json", payload, jsonSerializerOptions, ct);

        await EnsureSuccessAsync(response, "creating invoice", ct);

        return await response.Content.ReadFromJsonAsync<InvoiceResponse>(jsonSerializerOptions, ct)
               ?? throw CreateEmptyResponseException(response);
    }

    public async Task<InvoiceResponse> GetInvoiceAsync(long id, CancellationToken ct = default)
    {
        var url = GetUrl(id, "json");
        LogAction($"Fetching invoice {id}", "GET", url);

        var response = await httpClient.GetAsync(url, ct);
        await EnsureSuccessAsync(response, $"fetching invoice {id}", ct);

        return await response.Content.ReadFromJsonAsync<InvoiceResponse>(jsonSerializerOptions, ct)
               ?? throw CreateEmptyResponseException(response);
    }

    public async Task<Stream> GetInvoicePdfAsync(long id, CancellationToken ct = default)
    {
        var url = GetUrl(id, "pdf");
        LogAction($"Downloading PDF for invoice {id}", "GET", url);

        var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccessAsync(response, $"downloading PDF for invoice {id}", ct);

        return await response.Content.ReadAsStreamAsync(ct);
    }

    public async Task DeleteInvoiceAsync(long id, CancellationToken ct = default)
    {
        var url = GetUrl(id, "json");
        LogAction($"Deleting invoice {id}", "DELETE", url);

        var response = await httpClient.DeleteAsync(url, ct);
        await EnsureSuccessAsync(response, $"deleting invoice {id}", ct);
    }

    public async Task<IEnumerable<InvoiceResponse>> GetInvoicesAsync(InvoiceQueryParams? query = null, CancellationToken ct = default)
    {
        var queryString = query?.ToQueryString() ?? "";
        var url = GetUrl(null, "json", queryString);
        LogAction("Listing invoices", "GET", url);

        var response = await httpClient.GetAsync(url, ct);
        await EnsureSuccessAsync(response, "listing invoices", ct);

        return await response.Content.ReadFromJsonAsync<IEnumerable<InvoiceResponse>>(jsonSerializerOptions, ct) ?? [];
    }

    public async Task MarkAsPaidAsync(long id, CancellationToken ct = default)
        => await UpdateStatusAsync(id, InvoiceStatuses.Paid, ct);

    public async Task UpdateStatusAsync(long id, string status, CancellationToken ct = default)
    {
        var url = GetUrl(id);
        LogAction($"Updating status of invoice {id} to {status}", "PUT", url);

        var payload = new { api_token = _options.ApiToken, invoice = new { status } };
        var response = await httpClient.PutAsJsonAsync(url, payload, jsonSerializerOptions, ct);

        await EnsureSuccessAsync(response, $"updating status of invoice {id} to {status}", ct);
    }

    public async Task<InvoiceResponse> UpdateInvoiceAsync(long id, object invoiceFields, CancellationToken ct = default)
    {
        var url = GetUrl(id);
        LogAction($"Updating invoice {id}", "PUT", url);

        var payload = new { api_token = _options.ApiToken, invoice = invoiceFields };

        var response = await httpClient.PutAsJsonAsync(url, payload, jsonSerializerOptions, ct);

        await EnsureSuccessAsync(response, $"updating invoice {id}", ct);

        return await response.Content.ReadFromJsonAsync<InvoiceResponse>(jsonSerializerOptions, ct)
               ?? throw CreateEmptyResponseException(response);
    }

    public async Task SendByEmailAsync(long id, CancellationToken ct = default)
    {
        var url = $"invoices/{id}/send_by_email.json?api_token={_options.ApiToken}";
        LogAction($"Sending invoice {id} by email", "POST", url);

        var response = await httpClient.PostAsync(url, null, ct);

        await EnsureSuccessAsync(response, $"sending invoice {id} by email", ct);
    }

    // Helpers

    private void LogAction(string message, string method, string url)
    {
        var sanitizedUrl = SanitizeUrl(url);
        _logger.LogDebug("Fakturownia SDK: {Message} [{Method} {Url}]", message, method, sanitizedUrl);
    }

    private string SanitizeUrl(string url)
    {
        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(_options.ApiToken))
            return url;

        return url.Replace(_options.ApiToken, "***");
    }

    private string GetUrl(long? id = null, string extension = "json", string? extraQuery = null)
    {
        var path = id.HasValue ? $"invoices/{id}" : "invoices";
        var baseUrl = $"{path}.{extension}";

        var queryParams = new List<string> { $"api_token={_options.ApiToken}" };
        if (!string.IsNullOrEmpty(extraQuery))
            queryParams.Add(extraQuery.TrimStart('&'));

        return $"{baseUrl}?{string.Join("&", queryParams)}";
    }

    private async Task EnsureSuccessAsync(HttpResponseMessage response, string action, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            _logger.LogDebug("Fakturownia API: Successfully finished {Action} (Status: {StatusCode})", action, response.StatusCode);
            return;
        }

        var errorContent = await response.Content.ReadAsStringAsync(ct);

        _logger.LogError("Fakturownia API error during {Action}. Status: {StatusCode}, Reason: {Reason}, Body: {Body}",
            action, (int)response.StatusCode, response.ReasonPhrase, errorContent);

        throw new FakturowniaException(
            message: $"Fakturownia API returned an error while {action}: {response.ReasonPhrase}",
            statusCode: (int)response.StatusCode,
            responseBody: errorContent
        );
    }

    private static FakturowniaException CreateEmptyResponseException(HttpResponseMessage response)
        => new("Fakturownia API returned an empty response", (int)response.StatusCode);
}