namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

public interface IDonorsClient
{
    Task<MamisSolidarias.WebAPI.Donors.Endpoints.Donors.POST.Response?> CreateDonor(
        MamisSolidarias.WebAPI.Donors.Endpoints.Donors.POST.Request requestParameters,
        CancellationToken token = default
    );
    
    Task<MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Id.PUT.Response?> UpdateDonor(
        MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Id.PUT.Request requestParameters,
        CancellationToken token = default
    );
}