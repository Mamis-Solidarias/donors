using MamisSolidarias.WebAPI.Donors.Endpoints.Test;

namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

public interface IDonorsClient
{
    Task<Response?> GetTestAsync(Request requestParameters, CancellationToken token = default);
}