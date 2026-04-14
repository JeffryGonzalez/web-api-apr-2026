namespace Software.Api;

public class RemoteServiceClient(HttpClient httpClient)
{

    public async Task<RemoteServiceResponse> SendRequestAsync(RemoteServiceRequest request, CancellationToken token)
    {

        var httpResponse = await httpClient.PostAsJsonAsync<RemoteServiceRequest>("/pathtopostto", request, token);
        
        httpResponse.EnsureSuccessStatusCode(); 

        return await httpResponse.Content.ReadFromJsonAsync<RemoteServiceResponse>(cancellationToken: token) ?? new RemoteServiceResponse();

    }

public record RemoteServiceRequest
{
    public string FormType { get; set; }
    public string Body { get; set; }
}

public record RemoteServiceResponse
{
    public string Result { get; set; }
}