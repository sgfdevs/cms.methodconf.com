using FluentResults;
using MethodConf.Cms.Domain;

namespace MethodConf.Cms.Services.Interfaces;

public interface IConferenceIssueService
{
    public Task<Result<IssueWithResponse>> CreateIssue(Guid conferenceId, CreateIssue createIssue);
}
