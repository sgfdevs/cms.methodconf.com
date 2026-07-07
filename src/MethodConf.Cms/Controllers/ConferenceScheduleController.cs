using Asp.Versioning;
using MethodConf.Cms.Domain.Errors;
using MethodConf.Cms.Dtos;
using MethodConf.Cms.Mapping;
using MethodConf.Cms.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MethodConf.Cms.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route(RouteTemplates.ConferenceSchedule)]
public class ConferenceScheduleController(IConferenceScheduleService conferenceScheduleService, ScheduleMapper mapper) : Controller
{
    [HttpGet]
    public ActionResult<ConferenceScheduleResponseDto> GetSchedule(Guid conferenceId)
    {
        var result = conferenceScheduleService.GetSchedule(conferenceId);

        return result switch
        {
            { IsFailed: true } when result.Errors.Any(e => e is InvalidEntityIdError) => NotFound(result.Errors),
            { IsSuccess: true } => Ok(mapper.ToDto(result.Value)),
            _ => StatusCode(500)
        };
    }
}
