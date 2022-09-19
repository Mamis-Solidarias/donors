using MamisSolidarias.WebAPI.Donors.Endpoints.Test;

namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

public partial class DonorsClient
{
    public Task<Response?> GetTestAsync(Request requestParameters, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Get, "user", requestParameters.Name)
            .ExecuteAsync<Response>(token);

    }
}