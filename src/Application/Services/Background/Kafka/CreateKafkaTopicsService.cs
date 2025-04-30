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
    : EnsureTopicsCreatedService(kafkaOptions,kafkaEnvPrefixer, logger)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Starting background service with guard");
            await base.ExecuteAsync(stoppingToken);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Create topic background service failed");
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
