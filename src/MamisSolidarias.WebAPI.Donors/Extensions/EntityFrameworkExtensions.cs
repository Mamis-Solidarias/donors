using EntityFramework.Exceptions.PostgreSQL;
using MamisSolidarias.Infrastructure.Donors;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Donors.Extensions;

internal static class EntityFrameworkExtensions
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        var connectionString = configuration.GetConnectionString("DonorsDb");

        services.AddDbContext<DonorsDbContext>(
            t =>
            {
                t.UseNpgsql(connectionString, r => r.MigrationsAssembly("MamisSolidarias.WebAPI.Donors"))
                    .EnableSensitiveDataLogging(!env.IsProduction())
                    .EnableDetailedErrors(!env.IsProduction());
                t.UseExceptionProcessor();
            }
        );
    }
}