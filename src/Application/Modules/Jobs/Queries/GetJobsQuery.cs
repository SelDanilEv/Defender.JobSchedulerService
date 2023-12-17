using Defender.Common.DB.Pagination;
using Defender.Common.Errors;
using Defender.Common.Interfaces;
using Defender.JobSchedulerService.Application.Common.Interfaces;
using Defender.JobSchedulerService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Defender.JobSchedulerService.Application.Modules.Jobs.Querys;

public record GetJobsQuery : PaginationRequest, IRequest<PagedResult<ScheduledJob>>
{
    public string? Name { get; set; } = String.Empty;
};

public sealed class GetJobsQueryValidator : AbstractValidator<GetJobsQuery>
{
    public GetJobsQueryValidator()
    {
    }
}

public sealed class GetJobsQueryHandler :
    IRequestHandler<GetJobsQuery, PagedResult<ScheduledJob>>
{
    private readonly IAccountAccessor _accountAccessor;
    private readonly IJobManagementService _accountManagementService;

    public GetJobsQueryHandler(
        IAccountAccessor accountAccessor,
        IJobManagementService accountManagementService
        )
    {
        _accountAccessor = accountAccessor;
        _accountManagementService = accountManagementService;
    }

    public async Task<PagedResult<ScheduledJob>> Handle(
        GetJobsQuery request,
        CancellationToken cancellationToken)
    {
        return await _accountManagementService.GetJobsAsync(request, request.Name);
    }
}
