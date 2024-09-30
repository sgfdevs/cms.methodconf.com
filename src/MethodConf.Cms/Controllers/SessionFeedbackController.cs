using AutoMapper;
using MethodConf.Cms.Domain;
using MethodConf.Cms.Domain.Errors;
using MethodConf.Cms.Dtos;
using MethodConf.Cms.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MethodConf.Cms.Controllers;

[ApiController]
[Route(RouteTemplates.SessionFeedback)]
public class SessionFeedbackController(ISessionFeedbackService sessionFeedbackService, IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<ActionResult<SessionFeedbackResponseDto>> Create(Guid sessionId, CreateSessionFeedbackRequestDto request)
    {
        var createSessionFeedback = mapper.Map<CreateSessionFeedback>(request);

        var result = await sessionFeedbackService.Create(sessionId, createSessionFeedback);

        return result switch
        {
            { IsFailed: true } when result.Errors.Any(e => e is InvalidEntityIdError or IneligibleForFeedback) => BadRequest(result.Errors),
            { IsSuccess: true } => Ok(mapper.Map<SessionFeedbackResponseDto>(result.Value)),
            _ => StatusCode(500)
        };
    }
}
