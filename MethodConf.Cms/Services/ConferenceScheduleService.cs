using AutoMapper;
using FluentResults;
using MethodConf.Cms.Domain;
using MethodConf.Cms.Domain.Errors;
using MethodConf.Cms.Services.Interfaces;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace MethodConf.Cms.Services;

public class ConferenceScheduleService(IPublishedContentQuery publishedContentQuery, IScheduleGridGenerator scheduleGridGenerator, IMapper mapper) : IConferenceScheduleService
{
    public Result<ConferenceSchedule> GetSchedule(Guid conferenceId)
    {
        if (publishedContentQuery.Content(conferenceId) is not Conference conference)
        {
            return Result.Fail(new InvalidEntityIdError(conferenceId.ToString()));
        }

        var sessions = conference.FirstChild<Sessions>()?.Children.ToArray() ?? [];

        var scheduleItems = new List<ScheduleItem>();

        foreach (var sessionOrTrack in sessions)
        {
            switch (sessionOrTrack)
            {
                case Session session:
                    scheduleItems.Add(mapper.Map<SessionItem>(session));
                    continue;
                case Track track:
                    scheduleItems.Add(mapper.Map<TrackItem>(track));
                    continue;
            }
        }

        var scheduleGrid = scheduleGridGenerator.GenerateGrid(scheduleItems);

        return new ConferenceSchedule
        {
            ScheduleGrid = scheduleGrid,
        };
    }
}
