using Defender.Kafka;
using Defender.Kafka.BackgroundServices;
using Defender.Kafka.Configuration.Options;
using Defender.Kafka.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Defender.JobSchedulerService.Application.Services.Background.Kafka;

public class CreateKafkaTopicsService(
    IOptions<KafkaOptions> kafkaOptions,
    IKafkaEnvPrefixer kafkaEnvPrefixer,
    ILogger<CreateKafkaTopicsService> logger)
    : EnsureTopicsCreatedService(kafkaOptions, kafkaEnvPrefixer, logger)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        restart_go_to:

        try
        {
            
            logger.LogInformation("Starting background service with guard");
            logger.LogInformation("Kafka instance: {0}", kafkaOptions.Value.BootstrapServers);

            await Task.Delay(5000);
            logger.LogInformation("GOGOGO");

            await base.ExecuteAsync(stoppingToken);
        }
        catch(Exception ex)
        {
            logger.LogError(ex.InnerException, "Create topic background service failed");

            goto restart_go_to;
        }
    }

    protected override IEnumerable<string> Topics =>
        [
            Topic.TransactionStatusUpdates.GetName(),
            Topic.DistributedCache.GetName()
        ];

    protected override short ReplicationFactor => 1;

    protected override int NumPartitions => 3;
}
