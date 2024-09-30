using FluentResults;
using MethodConf.Cms.Domain;

namespace MethodConf.Cms.Services.Interfaces;

public interface ISessionFeedbackService
{
    public Task<Result<SessionFeedback>> Create(Guid sessionId, CreateSessionFeedback createSessionFeedback);
}
