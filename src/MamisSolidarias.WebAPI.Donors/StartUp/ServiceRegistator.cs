using EntityFramework.Exceptions.PostgreSQL;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using HotChocolate.Diagnostics;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Utils.Security;
using MamisSolidarias.WebAPI.Donors.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MamisSolidarias.WebAPI.Donors.StartUp;

internal static class ServiceRegistrator
{
    public static void Register(WebApplicationBuilder builder)
    {
        var connectionString = builder.Environment.EnvironmentName.ToLower() switch
        {
            "production" => builder.Configuration.GetConnectionString("Production"),
            _ => builder.Configuration.GetConnectionString("Development")
        };
        
        var dataProtectionKeysPath = builder.Configuration.GetValue<string>("DataProtectionKeysPath");
        if (!string.IsNullOrWhiteSpace(dataProtectionKeysPath))
        {
            builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath))
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });
        }

        builder.Services.AddOpenTelemetry(builder.Configuration, builder.Logging);
        
        builder.Services.AddFastEndpoints(t => t.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All);
        builder.Services.AddAuthenticationJWTBearer(
            builder.Configuration["Jwt:Key"],
            builder.Configuration["Jwt:Issuer"]
        );

        builder.Services.AddAuthorization(t => t.ConfigurePolicies(Services.Donors));
        
        builder.Services.AddDbContext<DonorsDbContext>(
            t =>
            {
                t.UseNpgsql(connectionString, r => r.MigrationsAssembly("MamisSolidarias.WebAPI.Donors"))
                    .EnableSensitiveDataLogging(!builder.Environment.IsProduction())
                    .EnableDetailedErrors(!builder.Environment.IsProduction());
                t.UseExceptionProcessor();
            }
        );

        builder.Services.AddSingleton(ConnectionMultiplexer.Connect($"{builder.Configuration["Redis:Host"]}:{builder.Configuration["Redis:Port"]}"));

        builder.Services.AddGraphQLServer()
            .AddQueryType<Queries.Donors>()
            .AddInstrumentation(t =>
            {
                t.Scopes = ActivityScopes.All;
                t.IncludeDocument = true;
                t.RequestDetails = RequestDetails.All;
                t.IncludeDataLoaderKeys = true;
            })
            .AddAuthorization()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .RegisterDbContext<DonorsDbContext>()
            .InitializeOnStartup()
            .PublishSchemaDefinition(t =>
                t.SetName($"{Services.Donors}gql")
                    .AddTypeExtensionsFromFile("./Stitching.graphql")
                    .PublishToRedis(builder.Configuration["GraphQl:GlobalSchemaName"],
                        sp => sp.GetRequiredService<ConnectionMultiplexer>()
                    )
            );

        if (!builder.Environment.IsProduction())
            builder.Services.AddSwaggerDoc(t => t.Title = "Donors");
    }
}