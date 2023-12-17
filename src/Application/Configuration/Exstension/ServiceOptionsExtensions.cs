using Defender.Common.Exstension;
using Defender.JobSchedulerService.Application.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.JobSchedulerService.Application.Configuration.Exstension;

public static class ServiceOptionsExtensions
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JobRunningOptions>(configuration.GetSection(nameof(JobRunningOptions)));

        return services;
    }
}