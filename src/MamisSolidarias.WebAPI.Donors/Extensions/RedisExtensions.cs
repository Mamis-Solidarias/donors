using StackExchange.Redis;

namespace MamisSolidarias.WebAPI.Donors.Extensions;

internal static class RedisExtensions
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("Redis");
        var connectionString = configuration.GetConnectionString("Redis");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            logger.LogWarning("Redis connection string is empty");
            return;
        }
        
        services.AddSingleton(ConnectionMultiplexer.Connect(connectionString));

    }
}