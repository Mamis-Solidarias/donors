using EntityFramework.Exceptions.PostgreSQL;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using HotChocolate.Diagnostics;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Utils.Security;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
        
        builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddConsoleExporter()
                .AddJaegerExporter(t =>
                {
                    var jaegerHost = builder.Configuration["OpenTelemetry:Jaeger:Host"];
                    if (jaegerHost is not null)
                        t.Endpoint = new Uri(jaegerHost);
                })
                .AddSource(builder.Configuration["OpenTelemetry:Name"])
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(
                            serviceName: builder.Configuration["OpenTelemetry:Name"], 
                            serviceVersion: builder.Configuration["OpenTelemetry:Version"]
                            )
                    )
                .AddHttpClientInstrumentation(t =>
                {
                    t.RecordException = true;
                    t.SetHttpFlavor = true;
                })
                .AddAspNetCoreInstrumentation(t=> t.RecordException = true)
                .AddEntityFrameworkCoreInstrumentation(t=> t.SetDbStatementForText = true);
        });        
        
        
        builder.Services.AddFastEndpoints(t=> t.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All);
        builder.Services.AddAuthenticationJWTBearer(
            builder.Configuration["JWT:Key"],
            builder.Configuration["JWT:Issuer"]
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
            .RegisterDbContext<DonorsDbContext>()
            .PublishSchemaDefinition(t => t.SetName($"{Services.Donors}gql"));;

        if (!builder.Environment.IsProduction())
            builder.Services.AddSwaggerDoc(t=> t.Title = "Donors");

    }
}