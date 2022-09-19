using FastEndpoints;
using FastEndpoints.Swagger;
using MamisSolidarias.Infrastructure.Donors;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Donors.StartUp;

internal static class MiddlewareRegistrator
{
    public static void Register(WebApplication app)
    {
        app.UseDefaultExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DonorsDbContext>();
            db.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3(t => t.ConfigureDefaults());
        }
    }
}