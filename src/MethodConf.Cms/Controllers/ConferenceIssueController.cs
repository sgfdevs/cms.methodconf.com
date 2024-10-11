using Asp.Versioning;
using AutoMapper;
using MethodConf.Cms.Domain;
using MethodConf.Cms.Domain.Errors;
using MethodConf.Cms.Dtos;
using MethodConf.Cms.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MethodConf.Cms.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route(RouteTemplates.ConferenceIssue)]
public class ConferenceIssueController(IConferenceIssueService conferenceIssueService, IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<ActionResult<CreateIssueResponseDto>> CreateIssue(Guid conferenceId, CreateIssueRequestDto request)
    {
        var createIssue = mapper.Map<CreateIssue>(request);

        var result = await conferenceIssueService.CreateIssue(conferenceId, createIssue);

        return result switch
        {
            { IsFailed: true } when result.Errors.Any(e => e is InvalidEntityIdError) => NotFound(result.Errors),
            { IsSuccess: true } => Ok(mapper.Map<CreateIssueResponseDto>(result.Value)),
            _ => StatusCode(500)
        };
    }
}
