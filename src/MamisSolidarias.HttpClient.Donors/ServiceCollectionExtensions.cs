using MamisSolidarias.HttpClient.Donors.Models;
using MamisSolidarias.HttpClient.Donors.DonorsClient;
using MamisSolidarias.HttpClient.Donors.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace MamisSolidarias.HttpClient.Donors;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// It registers the DonorsHttpClient using dependency injection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDonorsHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new DonorsConfiguration();
        configuration.GetSection("DonorsHttpClient").Bind(config);
        ArgumentNullException.ThrowIfNull(config.BaseUrl);
        ArgumentNullException.ThrowIfNull(config.Timeout);
        ArgumentNullException.ThrowIfNull(config.Retries);

        services.AddHttpContextAccessor();
        
        services.AddSingleton<IDonorsClient, DonorsClient.DonorsClient>();
        services.AddHttpClient("Donors", (services,client) =>
        {
            client.BaseAddress = new Uri(config.BaseUrl);
            client.Timeout = TimeSpan.FromMilliseconds(config.Timeout);
            
            var contextAccessor = services.GetService<IHttpContextAccessor>();
            if (contextAccessor is not null)
            {
                var authHeader = new HeaderService(contextAccessor).GetAuthorization();
                if (authHeader is not null)
                    client.DefaultRequestHeaders.Add("Authorization", authHeader);
            }
        })
            .AddTransientHttpErrorPolicy(t =>
            t.WaitAndRetryAsync(config.Retries,
                retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)))
        );
    }
}