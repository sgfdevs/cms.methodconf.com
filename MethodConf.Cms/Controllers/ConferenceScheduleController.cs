using Asp.Versioning;
using AutoMapper;
using MethodConf.Cms.Domain.Errors;
using MethodConf.Cms.Dtos;
using MethodConf.Cms.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Delivery.Controllers.Content;
using Umbraco.Cms.Api.Management.Controllers.PartialView.Snippet;

namespace MethodConf.Cms.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route(RouteTemplates.ConferenceSchedule)]
public class ConferenceScheduleController(IConferenceScheduleService conferenceScheduleService, IMapper mapper) : Controller
{
    [HttpGet]
    public ActionResult<ConferenceScheduleResponseDto> GetSchedule(Guid conferenceId)
    {
        var result = conferenceScheduleService.GetSchedule(conferenceId);

        return result switch
        {
            { IsFailed: true } when result.Errors.Any(e => e is InvalidEntityIdError) => NotFound(result.Errors),
            { IsSuccess: true } => Ok(mapper.Map<ConferenceScheduleResponseDto>(result.Value)),
            _ => StatusCode(500)
        };
    }
}
