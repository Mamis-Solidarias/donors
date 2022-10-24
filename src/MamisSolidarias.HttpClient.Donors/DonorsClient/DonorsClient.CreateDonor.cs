namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

/// <summary>
/// 
/// </summary>
partial class DonorsClient
{
    /// <inheritdoc />
    public Task<CreateDonorResponse?> CreateDonor(CreateDonorRequest req, CancellationToken token = default)
        => CreateRequest(HttpMethod.Post, "donors")
            .WithContent(new
            {
                req.Name,
                req.Email,
                req.Phone,
                req.IsGodFather
            })
            .ExecuteAsync<CreateDonorResponse>(token);


    /// <param name="Id">Id of the created donor</param>
    public sealed record CreateDonorResponse(int Id);
    

    /// <param name="Name">Name of the Donor</param>
    /// <param name="Email">Email of the donor. Optional</param>
    /// <param name="Phone">Phone of the donor. Optional</param>
    /// <param name="IsGodFather">Whether the sponsor is a godfather or not</param>
    public sealed record CreateDonorRequest(string Name, string Email, string Phone, bool IsGodFather);
}