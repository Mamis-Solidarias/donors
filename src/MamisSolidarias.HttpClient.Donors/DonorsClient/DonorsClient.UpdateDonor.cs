using MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Id.PUT;

namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

public partial class DonorsClient
{
    public Task<Response?> UpdateDonor(Request req, CancellationToken token = default)
        => CreateRequest(HttpMethod.Put, "donors", $"{req.Id}")
            .WithContent(new
            {
                req.Email,
                req.Name,
                req.IsGodFather,
                req.Phone
            })
            .ExecuteAsync<Response>(token);
}