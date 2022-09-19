using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace MamisSolidarias.HttpClient.Donors.Utils;

internal class ConfigurationFactory
{
    internal static IConfiguration GetDonorsConfiguration(
        string baseUrl = "https://test.com", int retries = 3, int timeout = 500
    )
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"DonorsHttpClient:BaseUrl", baseUrl},
            {"DonorsHttpClient:Retries", retries.ToString()},
            {"DonorsHttpClient:Timeout", timeout.ToString()}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }
}