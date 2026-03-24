using System.Net;
using System.Text;
using Moq;
using Moq.Protected;

namespace Biqydu.Fakturownia.Net.Tests;

public abstract class FakturowniaClientTestsBase
{
    protected Mock<HttpMessageHandler> HandlerMock = new();
    
    protected HttpClient CreateMockClient()
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