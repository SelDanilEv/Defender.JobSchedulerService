﻿using System.Reflection;
using Defender.JobSchedulerService.Application.Common.Interfaces;
using Defender.JobSchedulerService.Application.Common.Interfaces.Repositories;
using Defender.JobSchedulerService.Application.Common.Interfaces.Wrapper;
using Defender.JobSchedulerService.Infrastructure.Clients.Service;
using Defender.JobSchedulerService.Infrastructure.Clients.Service.Client;
using Defender.JobSchedulerService.Infrastructure.Repositories.ScheduledJobs;
using Defender.JobSchedulerService.Infrastructure.Services;
using Defender.JobSchedulerService.Infrastructure.Services.Hosted;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.JobSchedulerService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        RegisterServices(services);

        RegisterHostedServices(services);

        RegisterRepositories(services);

        RegisterApiClients(services, configuration);

        RegisterClientWrappers(services);

        return services;
    }

    private static void RegisterClientWrappers(IServiceCollection services)
    {
        services.AddTransient<IGenericClientWrapper, GenericClientWrapper>();
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

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddSingleton<IScheduledJobRepository, ScheduledJobRepository>();
    }

    private static void RegisterApiClients(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IGenericClient, GenericClient>(nameof(GenericClient));
    }

}