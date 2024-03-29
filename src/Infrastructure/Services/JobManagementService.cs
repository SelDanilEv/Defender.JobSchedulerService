using Defender.Common.DB.Model;
using Defender.Common.DB.Pagination;
using Defender.JobSchedulerService.Application.Common.Interfaces;
using Defender.JobSchedulerService.Application.Common.Interfaces.Repositories;
using Defender.JobSchedulerService.Application.Common.Interfaces.Wrapper;
using Defender.JobSchedulerService.Domain.Entities;

namespace Defender.JobSchedulerService.Infrastructure.Services;

public class JobManagementService(
        IScheduledJobRepository scheduledJobRepository,
        IGenericClientWrapper clientWrapper)
    : IJobManagementService, IJobRunningService
{
    public async Task<ICollection<ScheduledJob>> GetJobsToRunAsync()
    {
        var settings = PaginationSettings<ScheduledJob>.DefaultRequest();



        settings.SetupFindOptions(
            FindModelRequest<ScheduledJob>
                .Init(x => x.Schedule.NextStartTime, DateTime.UtcNow, FilterType.Lt)
                .Sort(x => x.Schedule.NextStartTime, SortType.Desc));

        var pagedResult = await scheduledJobRepository.GetScheduledJobsAsync(settings);

        return pagedResult.Items;
    }

    public async Task<PagedResult<ScheduledJob>> GetJobsAsync(
        PaginationRequest paginationRequest, string name = "")
    {
        var settings = PaginationSettings<ScheduledJob>.FromPaginationRequest(paginationRequest);

        if (!String.IsNullOrWhiteSpace(name))
        {
            var filterRequest = FindModelRequest<ScheduledJob>
                .Init(x => x.Name, name)
                .Sort(x => x.Schedule.NextStartTime, SortType.Desc);

            settings.SetupFindOptions(filterRequest);
        }

        return await scheduledJobRepository.GetScheduledJobsAsync(settings);
    }

    public async Task<ScheduledJob> CreateJobAsync(ScheduledJob scheduledJob)
    {
        return await scheduledJobRepository.CreateScheduledJobAsync(scheduledJob);
    }

    public async Task<ScheduledJob> UpdateJobAsync(ScheduledJob scheduledJob)
    {
        var updateRequest = UpdateModelRequest<ScheduledJob>
            .Init(scheduledJob.Id)
            .Set(x => x.Name, scheduledJob.Name)
            .Set(x => x.Tasks, scheduledJob.Tasks)
            .Set(x => x.Schedule, scheduledJob.Schedule);

        return await scheduledJobRepository.UpdateScheduledJobAsync(updateRequest);
    }

    public async Task DeleteJobAsync(Guid id)
    {
        await scheduledJobRepository.DeleteScheduledJobAsync(id);
    }

    public async Task RunJobAsync(ScheduledJob scheduledJob, bool force = false)
    {
        if (scheduledJob.ScheduleNextRun(force))
        {
            foreach (var task in scheduledJob.Tasks)
            {
                if (task == null || task.Url == null) continue;
                var _ = clientWrapper.SendRequestAsync(
                    task.Url,   
                    task.Method,
                    task.IsAuthorizationRequired);
            }

            var updateRequest = UpdateModelRequest<ScheduledJob>
                .Init(scheduledJob.Id)
                .Set(x => x.Schedule, scheduledJob.Schedule);

            await scheduledJobRepository.UpdateScheduledJobAsync(updateRequest);
        }
    }
}
