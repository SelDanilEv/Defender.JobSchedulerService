﻿using Defender.Common.DB.Model;
using Defender.Common.DB.Pagination;
using Defender.JobSchedulerService.Application.Common.Interfaces;
using Defender.JobSchedulerService.Application.Common.Interfaces.Repositories;
using Defender.JobSchedulerService.Application.Common.Interfaces.Wrapper;
using Defender.JobSchedulerService.Domain.Entities;

namespace Defender.JobSchedulerService.Infrastructure.Services;

public class JobManagementService : IJobManagementService, IJobRunningService
{
    private readonly IScheduledJobRepository _scheduledJobRepository;
    private readonly IGenericClientWrapper _clientWrapper;

    public JobManagementService(
        IScheduledJobRepository scheduledJobRepository,
        IGenericClientWrapper clientWrapper)
    {
        _scheduledJobRepository = scheduledJobRepository;
        _clientWrapper = clientWrapper;
    }

    public async Task<ICollection<ScheduledJob>> GetJobsToRunAsync()
    {
        var settings = PaginationSettings<ScheduledJob>.DefaultRequest();

        settings.AddFilter(
            FindModelRequest<ScheduledJob>
                .Init(x => x.Schedule.NextStartTime, DateTime.UtcNow, FilterType.Lt));

        var pagedResult = await _scheduledJobRepository.GetScheduledJobsAsync(settings);

        return pagedResult.Items;
    }

    public async Task<PagedResult<ScheduledJob>> GetJobsAsync(
        PaginationRequest paginationRequest, string name = "")
    {
        var settings = PaginationSettings<ScheduledJob>.FromPaginationRequest(paginationRequest);

        if (!String.IsNullOrWhiteSpace(name))
        {
            var filterRequest = FindModelRequest<ScheduledJob>.Init(x => x.Name, name);

            settings.AddFilter(filterRequest);
        }

        return await _scheduledJobRepository.GetScheduledJobsAsync(settings);
    }

    public async Task<ScheduledJob> CreateJobAsync(ScheduledJob scheduledJob)
    {
        return await _scheduledJobRepository.CreateScheduledJobAsync(scheduledJob);
    }

    public async Task<ScheduledJob> UpdateJobAsync(ScheduledJob scheduledJob)
    {
        var updateRequest = UpdateModelRequest<ScheduledJob>
            .Init(scheduledJob.Id)
            .UpdateField(x => x.Name, scheduledJob.Name)
            .UpdateField(x => x.Url, scheduledJob.Url)
            .UpdateField(x => x.Schedule, scheduledJob.Schedule)
            .UpdateField(x => x.Method, scheduledJob.Method)
            .UpdateField(x => x.IsAuthorizationRequired, scheduledJob.IsAuthorizationRequired);

        return await _scheduledJobRepository.UpdateScheduledJobAsync(updateRequest);
    }

    public async Task DeleteJobAsync(Guid id)
    {
        await _scheduledJobRepository.DeleteScheduledJobAsync(id);
    }

    public async Task RunJobAsync(ScheduledJob scheduledJob)
    {
        if (scheduledJob.ScheduleNextRun())
        {
            await _clientWrapper.SendRequestAsync(
                scheduledJob.Url,
                scheduledJob.Method,
                scheduledJob.IsAuthorizationRequired);

            var updateRequest = UpdateModelRequest<ScheduledJob>
                .Init(scheduledJob.Id)
                .UpdateField(x => x.Schedule, scheduledJob.Schedule);

            await _scheduledJobRepository.UpdateScheduledJobAsync(updateRequest);
        }
    }
}