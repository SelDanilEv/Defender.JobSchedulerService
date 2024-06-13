using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Defender.JobSchedulerService.Application.Common.Interfaces;
using Defender.JobSchedulerService.Application.Services.Hosted;
using Defender.JobSchedulerService.Application.Services;

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
