namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

partial class DonorsClient
{
    public Task<UpdateDonorResponse?> UpdateDonor(UpdateDonorRequest req, CancellationToken token = default)
        => CreateRequest(HttpMethod.Put, "donors", $"{req.Id}")
            .WithContent(new
            {
                req.Email,
                req.Name,
                req.IsGodFather,
                req.Phone
            })
            .ExecuteAsync<UpdateDonorResponse>(token);
    
    /// <param name="Id">ID of the donor</param>
    /// <param name="Name">Name of the donor</param>
    /// <param name="Email">Email of the donor</param>
    /// <param name="Phone">Phone number of the donor</param>
    /// <param name="IsGodFather">Is the donor a godfather</param>
    public sealed record UpdateDonorResponse(int Id, string Name, string? Email, string? Phone, bool IsGodFather);
    

    /// <param name="Id">Id of the donor</param>
    /// <param name="Name">Name of the donor</param>
    /// <param name="Email">Email of the donor</param>
    /// <param name="Phone">Phone number of the donor</param>
    /// <param name="IsGodFather">Is the donor a godfather?</param>
    public sealed record UpdateDonorRequest(int Id, string Name, string? Email, string? Phone, bool IsGodFather);
}