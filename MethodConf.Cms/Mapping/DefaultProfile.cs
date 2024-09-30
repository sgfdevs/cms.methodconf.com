using AutoMapper;
using MethodConf.Cms.Domain;
using MethodConf.Cms.Dtos;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace MethodConf.Cms.Mapping;

public class DefaultProfile : Profile
{
    public DefaultProfile()
    {
        CreateMap<ConferenceSchedule, ConferenceScheduleResponseDto>();
        CreateMap<Session, SessionItem>()
            .ForMember(dest => dest.Key, opts => opts.MapFrom(src => src.UrlSegment));
        CreateMap<Track, TrackItem>()
            .ForMember(dest => dest.Sessions, opt => opt.MapFrom((src, _) => src.Children<Session>() ?? []));
    }
}
