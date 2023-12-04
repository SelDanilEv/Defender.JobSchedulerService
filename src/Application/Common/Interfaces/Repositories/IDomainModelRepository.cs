using Defender.JobSchedulerService.Domain.Entities;

namespace Defender.JobSchedulerService.Application.Common.Interfaces.Repositories;

public interface IDomainModelRepository
{
    Task<DomainModel> GetDomainModelByIdAsync(Guid id);
}
