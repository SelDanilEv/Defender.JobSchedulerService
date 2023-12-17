using Defender.JobSchedulerService.Application.Common.Interfaces;
using Defender.JobSchedulerService.Application.Configuration.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Defender.JobSchedulerService.Infrastructure.Services.Hosted;

public class JobRunningBackgroundService : BackgroundService
{
    private readonly int _loopDelayMs;
    private readonly IJobRunningService _jobRunningService;

    public JobRunningBackgroundService(
        IOptions<JobRunningOptions> options,
        IJobRunningService jobRunningService)
    {
        _loopDelayMs = options.Value.LoopDelayMs;
        _jobRunningService = jobRunningService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_loopDelayMs, stoppingToken);

            var jobsToRun = await _jobRunningService.GetJobsToRunAsync();

            await Parallel.ForEachAsync(jobsToRun, async (job, cancellationToken) =>
            {
                await _jobRunningService.RunJobAsync(job);
            });
        }
    }
}
