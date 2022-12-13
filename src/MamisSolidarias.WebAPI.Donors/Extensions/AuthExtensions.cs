using FastEndpoints.Security;
using MamisSolidarias.Utils.Security;

namespace MamisSolidarias.WebAPI.Donors.Extensions;

internal static class AuthExtensions
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("Auth");

        var options = configuration.GetSection("Jwt").Get<JwtOptions>();

        if (options is null)
        {
            logger.LogError("Jwt options not found. Please check your configuration.");
            throw new ArgumentException("Jwt options not found. Please check your configuration.");
        }

        services.AddAuthenticationJWTBearer(options.Key, options.Issuer);
        services.AddAuthorization(t => t.ConfigurePolicies(Services.Donors));
    }

    private record JwtOptions(string Key, string Issuer);
}