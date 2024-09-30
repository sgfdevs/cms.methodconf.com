using AutoMapper;
using MethodConf.Cms.Dtos;
using MethodConf.Cms.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MethodConf.Cms.Controllers;

[ApiController]
[Route("api/conference/{conferenceId:guid}/schedule")]
public class ConferenceScheduleController(IConferenceScheduleService conferenceScheduleService, IMapper mapper) : Controller
{
    [HttpGet]
    public ActionResult<ConferenceScheduleResponseDto> GetSchedule(Guid conferenceId)
    {
        var conferenceSchedule = conferenceScheduleService.GetSchedule(conferenceId);

        if (conferenceSchedule is null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<ConferenceScheduleResponseDto>(conferenceSchedule));
    }
}
