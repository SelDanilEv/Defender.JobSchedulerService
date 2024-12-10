using Defender.Common.Configuration.Options.Kafka;
using Defender.Common.Kafka.BackgroundServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Defender.RiskGamesService.Application.Services.Background;

public class CreateKafkaTopicsService : EnsureTopicsCreatedService
{
    public CreateKafkaTopicsService(
        IOptions<KafkaOptions> kafkaOptions,
        ILogger<CreateKafkaTopicsService> logger) : base(kafkaOptions, logger)
    {
    }

    protected override IEnumerable<string> Topics =>
        [
        ];

    protected override short ReplicationFactor => 1;

    protected override int NumPartitions => 3;
}
