using Defender.Common.Errors;
using Defender.Common.Interfaces;
using Defender.JobSchedulerService.Application.Common.Interfaces;
using Defender.JobSchedulerService.Domain.Entities;
using Defender.JobSchedulerService.Domain.Enums;
using FluentValidation;
using MediatR;

namespace Defender.JobSchedulerService.Application.Modules.Jobs.Commands;

public record UpdateJobCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
    public SupportedHttpMethod Method { get; set; } = SupportedHttpMethod.Get;
    public DateTime StartDateTime { get; set; }
    public int EachMinutes { get; set; }
    public int EachHour { get; set; }
    public bool IsAuthorizationRequired { get; set; } = false;
};

public sealed class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
{
    public UpdateJobCommandValidator()
    {
        RuleFor(s => s.Id)
          .NotEmpty()
          .NotNull().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));

        RuleFor(s => s.Name)
                  .NotEmpty()
                  .NotNull().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));

        RuleFor(s => s.Url)
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

public sealed class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, Unit>
{
    private readonly IAccountAccessor _accountAccessor;
    private readonly IJobManagementService _accountManagementService;

    public UpdateJobCommandHandler(
        IAccountAccessor accountAccessor,
        IJobManagementService accountManagementService
        )
    {
        _accountAccessor = accountAccessor;
        _accountManagementService = accountManagementService;
    }

    public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = new ScheduledJob
        {
            Id = request.Id,
            Name = request.Name,
            Url = request.Url,
            Method = request.Method,
            IsAuthorizationRequired = request.IsAuthorizationRequired,
        };

        job.AddSchedule(request.StartDateTime, request.EachMinutes, request.EachHour);

        await _accountManagementService.UpdateJobAsync(job);

        return Unit.Value;
    }
}
