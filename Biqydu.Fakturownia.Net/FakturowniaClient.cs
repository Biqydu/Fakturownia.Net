using System.Net.Http.Json;
using Biqydu.Fakturownia.Net.Abstractions;
using Biqydu.Fakturownia.Net.Abstractions.Models;
using Biqydu.Fakturownia.Net.Abstractions.Models.Constants;
using Microsoft.Extensions.Options;

namespace Biqydu.Fakturownia.Net;

public class FakturowniaClient(HttpClient httpClient, IOptions<FakturowniaOptions> options)
    : IFakturowniaClient
{
    private readonly FakturowniaOptions _options = options.Value;

    public async Task<InvoiceResponse> CreateInvoiceAsync(InvoiceRequest request, CancellationToken ct = default)
    {
        var payload = new { api_token = _options.ApiToken, invoice = request };
        var response = await httpClient.PostAsJsonAsync("invoices.json", payload, ct);

        await EnsureSuccessAsync(response, "creating invoice", ct);

        return await response.Content.ReadFromJsonAsync<InvoiceResponse>(ct)
               ?? throw CreateEmptyResponseException(response);
    }

    public async Task<InvoiceResponse> GetInvoiceAsync(long id, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync(GetUrl(id, "json"), ct);

        await EnsureSuccessAsync(response, $"fetching invoice {id}", ct);
        
        return await response.Content.ReadFromJsonAsync<InvoiceResponse>(ct)
               ?? throw CreateEmptyResponseException(response);
    }

    public async Task<Stream> GetInvoicePdfAsync(long id, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync(GetUrl(id, "pdf"), HttpCompletionOption.ResponseHeadersRead, ct);

        await EnsureSuccessAsync(response, $"downloading PDF for invoice {id}", ct);

        return await response.Content.ReadAsStreamAsync(ct);
    }

    public async Task DeleteInvoiceAsync(long id, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync(GetUrl(id, "json"), ct);

        await EnsureSuccessAsync(response, $"deleting invoice {id}", ct);
    }

    public async Task<IEnumerable<InvoiceResponse>> GetInvoicesAsync(InvoiceQueryParams? query = null, CancellationToken ct = default)
    {
        var url = GetUrl(null, "json") + query?.ToQueryString();

        var response = await httpClient.GetAsync(url, ct);

        await EnsureSuccessAsync(response, "listing invoices", ct);

        return await response.Content.ReadFromJsonAsync<IEnumerable<InvoiceResponse>>(ct) ?? [];
    }

    public async Task MarkAsPaidAsync(long id, CancellationToken ct = default)
        => await UpdateStatusAsync(id, InvoiceStatus.Paid, ct);

    public async Task UpdateStatusAsync(long id, string status, CancellationToken ct = default)
    {
        var url = $"invoices/{id}.json?api_token={_options.ApiToken}";
        var payload = new { api_token = _options.ApiToken, invoice = new { status } };

        var response = await httpClient.PutAsJsonAsync(url, payload, ct);

        await EnsureSuccessAsync(response, $"updating status of invoice {id} to {status}", ct);
    }

    public async Task<InvoiceResponse> UpdateInvoiceAsync(long id, object invoiceFields, CancellationToken ct = default)
    {
        var url = $"invoices/{id}.json?api_token={_options.ApiToken}";
        var payload = new { api_token = _options.ApiToken, invoice = invoiceFields };

        var response = await httpClient.PutAsJsonAsync(url, payload, ct);

        await EnsureSuccessAsync(response, $"updating invoice {id}", ct);

        return await response.Content.ReadFromJsonAsync<InvoiceResponse>(ct)
               ?? throw CreateEmptyResponseException(response);
    }

    public async Task SendByEmailAsync(long id, CancellationToken ct = default)
    {
        var url = $"invoices/{id}/send_by_email.json?api_token={_options.ApiToken}";

        var response = await httpClient.PostAsync(url, null, ct);

        await EnsureSuccessAsync(response, $"sending invoice {id} by email", ct);
    }

    private string GetUrl(long? id, string extension)
    {
        var path = id is > 0 ? $"invoices/{id}" : "invoices";
        return $"{path}.{extension}?api_token={_options.ApiToken}";
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, string action, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode) return;

        var errorContent = await response.Content.ReadAsStringAsync(ct);
        throw new FakturowniaException(
            message: $"Fakturownia API returned an error while {action}: {response.ReasonPhrase}",
            statusCode: (int)response.StatusCode,
            responseBody: errorContent
        );
    }

    private static FakturowniaException CreateEmptyResponseException(HttpResponseMessage response)
        => new("Fakturownia API returned an empty response", (int)response.StatusCode);
}