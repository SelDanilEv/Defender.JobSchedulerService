using System.Reflection;
using Defender.JobSchedulerService.Application.Common.Interfaces.Services;
using Defender.JobSchedulerService.Application.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Defender.JobSchedulerService.Application.Services.Background;
using Defender.Kafka.Configuration.Options;
using Defender.Kafka.Extension;

namespace Defender.JobSchedulerService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.RegisterServices();

        services.RegisterHostedServices();

        services.RegisterKafkaServices(configuration);

        return services;
    }

    private static IServiceCollection RegisterKafkaServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddKafka(options =>
        {
            configuration.GetSection(nameof(KafkaOptions)).Bind(options);
        });

        return services;
    }

    private static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IJobManagementService, JobManagementService>();
        services.AddTransient<IJobRunningService, JobManagementService>();
    }

    private static void RegisterHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<JobRunningBackgroundService>();
    }
}
