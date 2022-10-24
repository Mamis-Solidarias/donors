namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

/// <summary>
/// 
/// </summary>
public interface IDonorsClient
{
    /// <summary>
    /// It creates a new donor
    /// </summary>
    /// <param name="requestParameters"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<DonorsClient.CreateDonorResponse?> CreateDonor(
        DonorsClient.CreateDonorRequest requestParameters,
        CancellationToken token
    );

    /// <summary>
    /// It updates an existing donor
    /// </summary>
    /// <param name="requestParameters"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<DonorsClient.UpdateDonorResponse?> UpdateDonor(
        DonorsClient.UpdateDonorRequest requestParameters,
        CancellationToken token
    );
}