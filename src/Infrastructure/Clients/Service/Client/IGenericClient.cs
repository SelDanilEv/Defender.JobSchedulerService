using Defender.Common.Clients.Base;
using Defender.JobSchedulerService.Domain.Enums;

namespace Defender.JobSchedulerService.Infrastructure.Clients.Service.Client;

public interface IGenericClient : IBaseServiceClient
{
    void SetUri(string uri);

    Task SendRequestAsync(SupportedHttpMethod httpMethod);
}
