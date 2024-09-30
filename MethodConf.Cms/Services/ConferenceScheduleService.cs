using AutoMapper;
using MethodConf.Cms.Domain;
using MethodConf.Cms.Services.Interfaces;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace MethodConf.Cms.Services;

public class ConferenceScheduleService(IUmbracoContextFactory umbracoContextFactory, IScheduleGridGenerator scheduleGridGenerator, IMapper mapper) : IConferenceScheduleService
{
    public ConferenceSchedule? GetSchedule(Guid conferenceId)
    {
        using var contextReference = umbracoContextFactory.EnsureUmbracoContext();
        var content = contextReference.UmbracoContext.Content ?? throw new InvalidOperationException();

        var conference = content.GetById(conferenceId);

        if (conference is null)
        {
            return null;
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
