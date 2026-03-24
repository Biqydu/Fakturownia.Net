using System.Net;
using Biqydu.Fakturownia.Net.Abstractions;
using Biqydu.Fakturownia.Net.Abstractions.Models;
using Biqydu.Fakturownia.Net.Abstractions.Models.Enums;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace Biqydu.Fakturownia.Net.Tests;

public class FakturowniaClientTests : FakturowniaClientTestsBase
{
    private readonly IOptions<FakturowniaOptions> _options = Options.Create(new FakturowniaOptions 
    { 
        ApiToken = "test-token", 
        Subdomain = "biqydu" 
    });

    [Fact]
    public async Task GetInvoiceAsync_ShouldReturnCorrectData_WhenResponseIsOk()
    {
        // Arrange
        const string jsonResponse = "{ \"id\": 123, \"number\": \"FV/1/2026\", \"price_gross\": 123.45 }";
        SetupResponse(HttpStatusCode.OK, jsonResponse);
        
        var client = new FakturowniaClient(CreateMockClient(), _options);

        // Act
        var result = await client.GetInvoiceAsync(123);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(123);
        result.Number.Should().Be("FV/1/2026");
        result.TotalPriceGross.Should().Be(123.45m);
    }
    
    [Fact]
    public async Task SendByEmailAsync_ShouldBuildCorrectUrl()
    {
        // Arrange
        SetupResponse(HttpStatusCode.OK, "{}");
        var client = new FakturowniaClient(CreateMockClient(), _options);

        // Act
        await client.SendByEmailAsync(123);

        // Assert
        HandlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post &&
                req.RequestUri!.ToString().Contains("invoices/123/send_by_email.json") &&
                req.RequestUri.ToString().Contains("api_token=test-token")),
            ItExpr.IsAny<CancellationToken>()
        );
    }
    
    
    
    [Fact]
    public async Task CreateInvoiceAsync_ShouldThrowFakturowniaException_WhenApiReturnsError()
    {
        // Arrange
        const string errorJson = "{ \"error\": \"Invalid API Token\" }";
        SetupResponse(HttpStatusCode.Unauthorized, errorJson);
    
        var client = new FakturowniaClient(CreateMockClient(), _options);
        var request = new InvoiceRequest 
        { 
            SellDate = "2026-03-24", 
            IssueDate = "2026-03-24", 
            SellerName = "S", 
            BuyerName = "B", 
            Positions = [] 
        };

        // Act
        var act = async () => await client.CreateInvoiceAsync(request);

        // Assert
        await act.Should().ThrowAsync<FakturowniaException>()
            .Where(ex => ex.StatusCode == 401)
            .Where(ex => ex.ResponseBody != null && ex.ResponseBody.Contains("Invalid API Token"));
    }
    
    
    [Fact]
    public async Task GetInvoicesAsync_ShouldIncludeQueryParameters_InUrl()
    {
        // Arrange
        SetupResponse(HttpStatusCode.OK, "[]");
        var client = new FakturowniaClient(CreateMockClient(), _options);
        var query = new InvoiceQueryParams 
        { 
            Period = "this_month", 
            Status = "paid" 
        };

        // Act
        await client.GetInvoicesAsync(query);

        // Assert
        HandlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri!.ToString().Contains("period=this_month") &&
                req.RequestUri.ToString().Contains("status=paid")),
            ItExpr.IsAny<CancellationToken>()
        );
    }
    
    [Fact]
    public async Task MarkAsPaidAsync_ShouldSendCorrectJsonPayload()
    {
        // Arrange
        SetupResponse(HttpStatusCode.OK, "{}");
        var client = new FakturowniaClient(CreateMockClient(), _options);

        // Act
        await client.MarkAsPaidAsync(456);

        // Assert
        HandlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Put &&
                req.RequestUri!.ToString().Contains("invoices/456.json")),
            ItExpr.IsAny<CancellationToken>()
        );
        
        var sentRequest = (HttpRequestMessage)HandlerMock.Invocations
            .First(x => x.Method.Name == "SendAsync").Arguments[0];
    
        var content = await sentRequest.Content!.ReadAsStringAsync();
        content.Should().Contain("\"status\":\"paid\"");
    }
    
    [Fact]
    public async Task CreateInvoiceAsync_ShouldSerializeIncomeKind_AsNumberString()
    {
        // Arrange
        SetupResponse(HttpStatusCode.Created, "{ \"id\": 1 }");
        var client = new FakturowniaClient(CreateMockClient(), _options);
        var request = new InvoiceRequest 
        { 
            Income = IncomeKind.Expense, 
            SellDate = "2026-03-24", 
            IssueDate = "2026-03-24", 
            SellerName = "S", 
            BuyerName = "B", 
            Positions = [] 
        };

        // Act
        await client.CreateInvoiceAsync(request);

        // Assert
        var invocation = HandlerMock.Invocations.Single(x => x.Method.Name == "SendAsync");
        var requestMessage = (HttpRequestMessage)invocation.Arguments[0];
        
        var requestBody = await requestMessage.Content!.ReadAsStringAsync();
    
        requestBody.Should().Contain("\"income\":\"0\"");
    }
    
    [Fact]
    public async Task GetInvoicePdfAsync_ShouldReturnStream()
    {
        // Arrange
        var pdfBytes = "fake-pdf-content"u8.ToArray();
        HandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(pdfBytes)
            });

        var client = new FakturowniaClient(CreateMockClient(), _options);

        // Act
        var stream = await client.GetInvoicePdfAsync(123);
        var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();

        // Assert
        content.Should().Be("fake-pdf-content");
    }
    
    
    [Fact]
    public async Task UpdateInvoiceAsync_ShouldSendCorrectFields()
    {
        // Arrange
        const string jsonResponse = "{ \"id\": 123, \"buyer_name\": \"New Name\" }";
        SetupResponse(HttpStatusCode.OK, jsonResponse);
        var client = new FakturowniaClient(CreateMockClient(), _options);
        var updateFields = new { buyer_name = "New Name" };

        // Act
        var result = await client.UpdateInvoiceAsync(123, updateFields);

        // Assert
        var invocation = HandlerMock.Invocations.Single(x => x.Method.Name == "SendAsync");
        var requestMessage = (HttpRequestMessage)invocation.Arguments[0];
        var requestBody = await requestMessage.Content!.ReadAsStringAsync();

        requestMessage.Method.Should().Be(HttpMethod.Put);
        requestBody.Should().Contain("\"buyer_name\":\"New Name\"");
        result.BuyerName.Should().Be("New Name");
    }
    
    
    [Fact]
    public async Task DeleteInvoiceAsync_ShouldExecuteDeleteRequest()
    {
        // Arrange
        SetupResponse(HttpStatusCode.OK, "");
        var client = new FakturowniaClient(CreateMockClient(), _options);

        // Act
        await client.DeleteInvoiceAsync(123);

        // Assert
        HandlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => 
                req.Method == HttpMethod.Delete && 
                req.RequestUri!.ToString().Contains("invoices/123.json")),
            ItExpr.IsAny<CancellationToken>()
        );
    }
    
    [Fact]
    public async Task CreateInvoiceAsync_ShouldHandleValidationError_422()
    {
        // Arrange
        const string validationError = "{ \"error\": \"Positions can't be empty\" }";
        SetupResponse(HttpStatusCode.UnprocessableEntity, validationError);
        var client = new FakturowniaClient(CreateMockClient(), _options);

        // Act
        var act = async () => await client.CreateInvoiceAsync(new InvoiceRequest { 
            SellDate = "2026", IssueDate = "2026", SellerName = "S", BuyerName = "B", Positions = [] 
        });

        // Assert
        await act.Should().ThrowAsync<FakturowniaException>()
            .Where(ex => ex.StatusCode == 422)
            .Where(ex => ex.ResponseBody != null && ex.ResponseBody.Contains("Positions can't be empty"));
    }
    
    
    [Fact]
    public async Task GetInvoicesAsync_ShouldReturnEmptyEnumerable_WhenApiReturnsNull()
    {
        // Arrange
        SetupResponse(HttpStatusCode.OK, "null"); // API czasem płata figle
        var client = new FakturowniaClient(CreateMockClient(), _options);

        // Act
        var result = await client.GetInvoicesAsync();

        // Assert
        var invoiceResponses = result as InvoiceResponse[];
        invoiceResponses.Should().NotBeNull();
        invoiceResponses.Should().BeEmpty();
    }
}