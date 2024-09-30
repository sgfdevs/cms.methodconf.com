using FluentResults;
using MethodConf.Cms.Domain;

namespace MethodConf.Cms.Services.Interfaces;

public interface IConferenceScheduleService
{
    Result<ConferenceSchedule> GetSchedule(Guid conferenceId);
}
