using FluentResults;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace MethodConf.Cms.Domain.Errors;

public class IneligibleForFeedback : Error
{
    public IneligibleForFeedback(Session session) : base($"Session {session.Key} is not eligible for feedback")
    {

    }
}
