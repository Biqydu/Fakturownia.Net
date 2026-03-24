using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions;

public interface IFakturowniaClient
{
    // Documents
    Task<InvoiceResponse> CreateInvoiceAsync(InvoiceRequest request, CancellationToken ct = default);
    Task<InvoiceResponse> GetInvoiceAsync(long id, CancellationToken ct = default);
    Task<InvoiceResponse> UpdateInvoiceAsync(long id, object invoiceFields, CancellationToken ct = default); 
    Task DeleteInvoiceAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<InvoiceResponse>> GetInvoicesAsync(InvoiceQueryParams? query = null, CancellationToken ct = default);
    
    // Actions
    Task MarkAsPaidAsync(long id, CancellationToken ct = default);
    Task UpdateStatusAsync(long id, string status, CancellationToken ct = default);
    Task SendByEmailAsync(long id, CancellationToken ct = default); 
    
    // Files
    Task<Stream> GetInvoicePdfAsync(long id, CancellationToken ct = default);
}