using MamisSolidarias.Utils.Http;

namespace MamisSolidarias.HttpClient.Donors.DonorsClient;

public sealed partial class DonorsClient : IDonorsClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public DonorsClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    private ReadyRequest CreateRequest(HttpMethod httpMethod,params string[] urlParams)
    {
        var client = _httpClientFactory.CreateClient("Donors");
        var request = new HttpRequestMessage(httpMethod, string.Join('/', urlParams));
        
        return new ReadyRequest(client,request);
    }
}