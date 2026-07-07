using AutoMapper;
using MethodConf.Cms.Domain;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace MethodConf.Cms.Mapping;

public class SessionUrlSegmentResolver : IValueResolver<Session, SessionItem, string>
{
    private readonly IDocumentUrlService _documentUrlService;

    public SessionUrlSegmentResolver(IDocumentUrlService documentUrlService)
    {
        _documentUrlService = documentUrlService;
    }

    public string Resolve(Session source, SessionItem destination, string destMember, ResolutionContext context)
    {
        return _documentUrlService.GetUrlSegment(source.Key, "", false) ?? source.Key.ToString();
    }
}
