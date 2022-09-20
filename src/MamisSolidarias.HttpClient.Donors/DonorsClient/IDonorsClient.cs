
namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

public interface IDonorsClient
{
    Task<MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Response?> CreateDonor(
        MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Request requestParameters,
        CancellationToken token = default
    );
}