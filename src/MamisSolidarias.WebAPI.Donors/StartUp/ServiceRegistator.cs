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
                .AddJaegerExporter()
                .AddSource(builder.Configuration["Service:Name"])
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(
                            serviceName: builder.Configuration["Service:Name"], 
                            serviceVersion: builder.Configuration["Service:Version"]
                            )
                    )
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation();
        });        
        
        
        builder.Services.AddFastEndpoints(t=> t.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All);
        builder.Services.AddAuthenticationJWTBearer(builder.Configuration["JWT:Key"]);
        builder.Services.AddAuthorization(t =>
        {
            t.ConfigurePolicies(Services.Donors);
        });
        
        
        builder.Services.AddDbContext<DonorsDbContext>(
            t =>
            {
                t.UseNpgsql(connectionString, r => { r.MigrationsAssembly("MamisSolidarias.WebAPI.Donors"); })
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
            .RegisterDbContext<DonorsDbContext>();

        if (!builder.Environment.IsProduction())
            builder.Services.AddSwaggerDoc(t=> t.Title = "Donors");

        builder.Services.AddCors();
    }
}