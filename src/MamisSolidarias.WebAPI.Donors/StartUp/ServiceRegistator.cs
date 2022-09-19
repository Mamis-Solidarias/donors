using EntityFramework.Exceptions.PostgreSQL;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
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
        
        
        builder.Services.AddFastEndpoints();
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

        if (!builder.Environment.IsProduction())
            builder.Services.AddSwaggerDoc(t=> t.Title = "Donors");
    }
}