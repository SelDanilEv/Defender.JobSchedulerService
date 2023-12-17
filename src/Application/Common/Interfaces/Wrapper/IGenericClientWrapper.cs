using Defender.JobSchedulerService.Domain.Enums;

namespace Defender.JobSchedulerService.Application.Common.Interfaces.Wrapper;
public interface IGenericClientWrapper
{
    Task SendRequestAsync(
        string url,
        SupportedHttpMethod httpMethod = SupportedHttpMethod.Get,
        bool isAuthRequired = false);
}
