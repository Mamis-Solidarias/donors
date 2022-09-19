using MamisSolidarias.HttpClient.Donors.Models;
using MamisSolidarias.HttpClient.Donors.DonorsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace MamisSolidarias.HttpClient.Donors;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// It registers the DonorsHttpClient using dependency injection
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDonorsHttpClient(this WebApplicationBuilder builder)
    {
        var configuration = new DonorsConfiguration();
        builder.Configuration.GetSection("DonorsHttpClient").Bind(configuration);
        ArgumentNullException.ThrowIfNull(configuration.BaseUrl);
        ArgumentNullException.ThrowIfNull(configuration.Timeout);
        ArgumentNullException.ThrowIfNull(configuration.Retries);

        builder.Services.AddSingleton<IDonorsClient, DonorsClient.DonorsClient>();
        builder.Services.AddHttpClient("Donors", client =>
        {
            client.BaseAddress = new Uri(configuration.BaseUrl);
            client.Timeout = TimeSpan.FromMilliseconds(configuration.Timeout);
            client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        })
            .AddTransientHttpErrorPolicy(t =>
            t.WaitAndRetryAsync(configuration.Retries,
                retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)))
        );
    }
}