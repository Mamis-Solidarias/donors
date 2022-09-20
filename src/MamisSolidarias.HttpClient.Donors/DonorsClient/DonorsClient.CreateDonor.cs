using MamisSolidarias.WebAPI.Donors.Endpoints.Donors;
using MamisSolidarias.WebAPI.Donors.Endpoints.Donors.POST;

namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

public partial class DonorsClient
{
    public Task<Response?> CreateDonor(Request req, CancellationToken token = default)
        => CreateRequest(HttpMethod.Post, "donors")
            .WithContent(new
            {
                req.Name,
                req.Email,
                req.Phone,
                req.IsGodFather
            })
            .ExecuteAsync<Response>(token);
}