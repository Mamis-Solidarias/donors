using HotChocolate.Diagnostics;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Utils.Security;
using StackExchange.Redis;

namespace MamisSolidarias.WebAPI.Donors.Extensions;

internal static class GraphQlExtensions
{
    public static void AddGraphQl(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("GraphQL");
        var options = configuration.GetSection("GraphQl").Get<GraphQlOptions>();

        var builder = services.AddGraphQLServer()
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
            .InitializeOnStartup();

        if (options is null)
        {
            logger.LogWarning("GraphQl options not found");
            return;
        }
        
        builder.PublishSchemaDefinition(t =>
            t.SetName($"{Services.Donors}gql")
                .AddTypeExtensionsFromFile("./Stitching.graphql")
                .PublishToRedis(options.GlobalSchemaName,
                    sp => sp.GetRequiredService<ConnectionMultiplexer>()
                )
        );
    }
    
    private sealed record GraphQlOptions(string GlobalSchemaName);
}