using Defender.JobSchedulerService.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.JobSchedulerService.Domain.Entities;

public class ScheduledTask
{
    public string? Url { get; set; }
    [BsonRepresentation(BsonType.String)]
    public SupportedHttpMethod Method { get; set; }
    public bool IsAuthorizationRequired { get; set; }
}
