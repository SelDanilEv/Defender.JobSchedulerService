using System.Reflection;
using Defender.JobSchedulerService.Application.Common.Interfaces.Services;
using Defender.JobSchedulerService.Application.Services;
using Defender.JobSchedulerService.Application.Services.Hosted;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.JobSchedulerService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        RegisterServices(services);

        RegisterHostedServices(services);

        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IJobManagementService, JobManagementService>();
        services.AddTransient<IJobRunningService, JobManagementService>();
    }

    private static void RegisterHostedServices(IServiceCollection services)
    {
        services.AddHostedService<JobRunningBackgroundService>();
    }
}
