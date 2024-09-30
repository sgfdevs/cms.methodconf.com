using MethodConf.Cms.Domain;

namespace MethodConf.Cms.Services.Interfaces;

public interface IConferenceScheduleService
{
    ConferenceSchedule? GetSchedule(Guid conferenceId);
}
