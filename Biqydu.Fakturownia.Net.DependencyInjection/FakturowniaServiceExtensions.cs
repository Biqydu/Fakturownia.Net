using System.Net;
using Biqydu.Fakturownia.Net.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Biqydu.Fakturownia.Net.DependencyInjection;

public static class FakturowniaServiceExtensions
{
    public static IServiceCollection AddFakturownia(this IServiceCollection services,
        Action<FakturowniaOptions> configure)
    {
        services.Configure(configure);
        
        services.AddHttpClient<IFakturowniaClient, FakturowniaClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<FakturowniaOptions>>().Value;
        
                if (string.IsNullOrWhiteSpace(options.Subdomain))
                    throw new ArgumentException("Fakturownia Subdomain must be provided.");
            
                if (string.IsNullOrWhiteSpace(options.ApiToken))
                    throw new ArgumentException("Fakturownia API Token must be provided.");
            
                client.BaseAddress = new Uri($"https://{options.Subdomain}.fakturownia.pl/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(GetRetryPolicy()); 

        return services;
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() 
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(3, retryAttempt => 
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); 
    }
}