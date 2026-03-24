namespace Biqydu.Fakturownia.Net.Abstractions;

public class FakturowniaException(string message, int? statusCode = null, string? responseBody = null)
    : Exception(message)
{
    public int? StatusCode { get; } = statusCode;
    public string? ResponseBody { get; } = responseBody;
}