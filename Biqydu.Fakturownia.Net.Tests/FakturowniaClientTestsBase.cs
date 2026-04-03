using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace Biqydu.Fakturownia.Net.Tests;

public abstract class FakturowniaClientTestsBase
{
    protected readonly Mock<HttpMessageHandler> HandlerMock = new();
    protected readonly Mock<ILogger<FakturowniaClient>> LoggerMock  = new();
    
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };
    private readonly IOptions<FakturowniaOptions> _options = Options.Create(new FakturowniaOptions
    {
        ApiToken = "test-token",
        Subdomain = "biqydu"
    });
    
    protected FakturowniaClient CreateClient()
    {
        return new FakturowniaClient(
            CreateMockClient(), 
            _options, 
            _jsonOptions,
            LoggerMock.Object);
    }

    private HttpClient CreateMockClient()
    {
        return new HttpClient(HandlerMock.Object)
        {
            BaseAddress = new Uri("https://biqydu.fakturownia.pl/")
        };
    }

    protected void SetupResponse(HttpStatusCode code, string jsonContent)
    {
        HandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = code,
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            });
    }
}