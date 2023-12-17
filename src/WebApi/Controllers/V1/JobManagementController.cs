using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Defender.JobSchedulerService.Application.Modules.Jobs.Commands;
using Defender.JobSchedulerService.Application.Modules.Jobs.Querys;
using Defender.Common.DB.Pagination;
using Defender.JobSchedulerService.Domain.Entities;
using Defender.Common.Attributes;
using Defender.Common.Models;

namespace Defender.JobSchedulerService.WebUI.Controllers.V1;

public class JobManagementController : BaseApiController
{
    public JobManagementController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet("get")]
    //[Auth(Roles.Admin)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<PagedResult<ScheduledJob>> GetJobsAsync([FromQuery] GetJobsQuery query)
    {
        return await ProcessApiCallAsync<GetJobsQuery, PagedResult<ScheduledJob>>(query);
    }

    [HttpPost("create")]
    //[Auth(Roles.Admin)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task CreateJobAsync([FromBody] CreateJobCommand command)
    {
        await ProcessApiCallAsync(command);
    }

    [HttpPut("update")]
    //[Auth(Roles.Admin)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task UpdateJobAsync([FromBody] UpdateJobCommand command)
    {
        await ProcessApiCallAsync(command);
    }

    [HttpDelete("delete")]
    //[Auth(Roles.Admin)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task DeleteJobAsync([FromBody] DeleteJobCommand command)
    {
        await ProcessApiCallAsync(command);
    }

}
