using AutoMapper;
using Defender.Common.Interfaces;
using Defender.Common.Wrapper.Internal;
using Defender.JobSchedulerService.Application.Common.Interfaces.Wrapper;
using Defender.JobSchedulerService.Domain.Enums;
using Defender.JobSchedulerService.Infrastructure.Clients.Service.Client;

namespace Defender.JobSchedulerService.Infrastructure.Clients.Service;

public class GenericClientWrapper : BaseInternalSwaggerWrapper, IGenericClientWrapper
{
    private readonly IMapper _mapper;
    private readonly IGenericClient _client;

    public GenericClientWrapper(
        IGenericClient client,
        IAuthenticationHeaderAccessor authenticationHeaderAccessor,
        IMapper mapper) :
            base(client, authenticationHeaderAccessor)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task SendRequestAsync(
        string url,
        SupportedHttpMethod httpMethod = SupportedHttpMethod.Get,
        bool isAuthRequired = false)
    {
        await ExecuteSafelyAsync(async () =>
        {
            _client.SetUri(url);
            await _client.SendRequestAsync(httpMethod);
        }, isAuthRequired ? AuthorizationType.Service : AuthorizationType.WithoutAuthorization);
    }
}
