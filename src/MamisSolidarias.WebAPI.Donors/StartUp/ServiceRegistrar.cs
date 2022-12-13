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

internal static class ServiceRegistrar
{
    private static ILoggerFactory CreateLoggerFactory(IConfiguration configuration) =>
        LoggerFactory.Create(loggingBuilder => loggingBuilder
            .AddConfiguration(configuration)
            .AddConsole()
        );
    
    public static void Register(WebApplicationBuilder builder)
    {
        var loggerFactory = CreateLoggerFactory(builder.Configuration);

        builder.Services.AddEntityFramework(builder.Configuration,builder.Environment);
        
        
        

        builder.Services.AddOpenTelemetry(builder.Configuration, builder.Logging);
        
        builder.Services.AddFastEndpoints(t => t.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All);
        builder.Services.AddAuthenticationJWTBearer(
            builder.Configuration["Jwt:Key"],
            builder.Configuration["Jwt:Issuer"]
        );

        builder.Services.AddAuthorization(t => t.ConfigurePolicies(Services.Donors));
        
        

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