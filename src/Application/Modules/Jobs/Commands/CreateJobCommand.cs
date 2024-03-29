﻿using Defender.Common.Errors;
using Defender.JobSchedulerService.Application.Common.Interfaces;
using Defender.JobSchedulerService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Defender.JobSchedulerService.Application.Modules.Jobs.Commands;

public record CreateJobCommand : IRequest<Unit>
{
    public string? Name { get; set; }
    public List<ScheduledTask>? Tasks { get; set; }
    public DateTime StartDateTime { get; set; }
    public int EachMinutes { get; set; }
    public int EachHour { get; set; }
};

public sealed class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(s => s.Name)
          .NotEmpty()
          .NotNull().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));

        RuleFor(command => command)
            .Must(command => command.EachMinutes > 0 || command.EachHour > 0)
            .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));

        RuleFor(command => command.StartDateTime)
            .Must(BeInFuture)
            .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));
    }

    private static bool BeInFuture(DateTime startDateTime)
    {
        return startDateTime > DateTime.UtcNow;
    }
}

public sealed class CreateJobCommandHandler(
        IJobManagementService accountManagementService)
    : IRequestHandler<CreateJobCommand, Unit>
{
    public async Task<Unit> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = new ScheduledJob()
        {
            Name = request.Name,
            Tasks = request.Tasks ?? new List<ScheduledTask>()
        };

        job.AddSchedule(request.StartDateTime, request.EachMinutes, request.EachHour);

        await accountManagementService.CreateJobAsync(job);

        return Unit.Value;
    }
}
