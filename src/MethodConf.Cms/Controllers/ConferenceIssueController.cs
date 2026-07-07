using Asp.Versioning;
using MethodConf.Cms.Domain;
using MethodConf.Cms.Domain.Errors;
using MethodConf.Cms.Dtos;
using MethodConf.Cms.Mapping;
using MethodConf.Cms.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MethodConf.Cms.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route(RouteTemplates.ConferenceIssue)]
public class ConferenceIssueController(IConferenceIssueService conferenceIssueService, IssueMapper mapper) : Controller
{
    [HttpPost]
    public async Task<ActionResult<CreateIssueResponseDto>> CreateIssue(Guid conferenceId, CreateIssueRequestDto request)
    {
        var createIssue = mapper.ToCreateIssue(request);

        var result = await conferenceIssueService.CreateIssue(conferenceId, createIssue);

        return result switch
        {
            { IsFailed: true } when result.Errors.Any(e => e is InvalidEntityIdError) => NotFound(result.Errors),
            { IsSuccess: true } => Ok(mapper.ToCreateIssueResponseDto(result.Value)),
            _ => StatusCode(500)
        };
    }
}
