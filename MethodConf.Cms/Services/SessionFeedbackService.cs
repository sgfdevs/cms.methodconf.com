using AutoMapper;
using FluentResults;
using MethodConf.Cms.Domain;
using MethodConf.Cms.Domain.Errors;
using MethodConf.Cms.Infrastructure;
using MethodConf.Cms.Services.Interfaces;
using Umbraco.Cms.Core;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace MethodConf.Cms.Services;

public class SessionFeedbackService(IPublishedContentQuery publishedContentQuery, AppDbContext dbContext, IMapper mapper) : ISessionFeedbackService
{

    public async Task<Result<SessionFeedback>> Create(Guid sessionId, CreateSessionFeedback createSessionFeedback)
    {
        if (publishedContentQuery.Content(sessionId) is not Session session)
        {
            return new InvalidEntityIdError(sessionId.ToString());
        }

        if (!session.IsEligibleForFeedback)
        {
            return new IneligibleForFeedback(session);
        }

        var sessionFeedback = mapper.Map<SessionFeedback>(createSessionFeedback);


        dbContext.SessionFeedback.Add(sessionFeedback);
        await dbContext.SaveChangesAsync();

        return sessionFeedback;
    }
}
