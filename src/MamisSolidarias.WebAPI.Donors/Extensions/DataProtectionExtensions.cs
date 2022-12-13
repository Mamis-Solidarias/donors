using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

namespace MamisSolidarias.WebAPI.Donors.Extensions;

internal static class DataProtectionExtensions
{
    public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("DataProtection");
        var dataProtectionKeysPath = configuration.GetValue<string>("DataProtectionKeysPath");
        if (string.IsNullOrWhiteSpace(dataProtectionKeysPath))
        {
            logger.LogWarning("DataProtectionKeysPath is not set. Security keys will not be persisted after application restarts");
            return;
        }
        
        services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath))
            .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });
    }
}