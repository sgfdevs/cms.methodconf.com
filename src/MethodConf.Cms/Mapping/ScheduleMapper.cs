using MethodConf.Cms.Domain;
using MethodConf.Cms.Dtos;
using Riok.Mapperly.Abstractions;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace MethodConf.Cms.Mapping;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ScheduleMapper(IDocumentUrlService documentUrlService)
{
    [MapperRequiredMapping(RequiredMappingStrategy.Both)]
    public partial ConferenceScheduleResponseDto ToDto(ConferenceSchedule src);

    [MapProperty(nameof(Session.Key), nameof(SessionItem.Key), Use = nameof(ResolveSessionKey))]
    public partial SessionItem ToSessionItem(Session src);

    [MapPropertyFromSource(nameof(TrackItem.Sessions), Use = nameof(MapTrackSessions))]
    public partial TrackItem ToTrackItem(Track src);

    private SessionItem[] MapTrackSessions(Track src)
    {
        var sessions = src.Children<Session>() ?? [];
        return sessions.Select(ToSessionItem).ToArray();
    }

    private string ResolveSessionKey(Guid key) =>
        documentUrlService.GetUrlSegment(key, "", false) ?? key.ToString();
}
