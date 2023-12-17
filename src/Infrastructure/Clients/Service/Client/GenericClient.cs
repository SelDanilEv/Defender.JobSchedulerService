using System.Net.Http.Headers;
using Defender.JobSchedulerService.Domain.Enums;

namespace Defender.JobSchedulerService.Infrastructure.Clients.Service.Client;

public class GenericClient : IGenericClient
{
    private readonly HttpClient _httpClient;
    private Uri? _uri;

    public GenericClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }

    public void SetUri(string uri)
    {
        _uri = new Uri(uri);
    }

    public async Task SendRequestAsync(SupportedHttpMethod httpMethod)
    {
        switch (httpMethod)
        {
            case SupportedHttpMethod.Get:
                await _httpClient.GetAsync(_uri);
                break;
            case SupportedHttpMethod.Post:
                await _httpClient.PostAsync(_uri, null);
                break;
        }
    }
}
