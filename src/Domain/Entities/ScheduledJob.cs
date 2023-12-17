using Defender.Common.Entities;
using Defender.JobSchedulerService.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Defender.JobSchedulerService.Domain.Entities;

public class ScheduledJob : IBaseModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Url { get; set; }
    [BsonRepresentation(BsonType.String)]
    public SupportedHttpMethod Method { get; set; }
    public Schedule Schedule { get; set; } = new Schedule();
    public bool IsAuthorizationRequired { get; set; }

    public ScheduledJob AddSchedule(DateTime startDate, int eachMinute, int eachHour)
    {
        Schedule.NextStartTime = startDate;
        Schedule.LastStartedDate = DateTime.MinValue;
        Schedule.EachMinutes = eachMinute;
        Schedule.EachHour = eachHour;

        return this;
    }

    public bool ScheduleNextRun()
    {
        if (Schedule == null) return false;

        if (Schedule.NextStartTime > DateTime.UtcNow) return false;

        Schedule.LastStartedDate = Schedule.NextStartTime;
        Schedule.NextStartTime = Schedule.LastStartedDate;

        while(Schedule.NextStartTime < DateTime.UtcNow)
        {
            Schedule.NextStartTime = Schedule.NextStartTime
                .AddMinutes(Schedule.EachMinutes)
                .AddHours(Schedule.EachHour);
        }

        return true;
    }
}
