using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;

namespace MethodConf.Cms.Filters;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SpeakerFilter : IFilterHandler, IContentIndexHandler
{
    private const string SpeakerSpecifier = "speaker:";
    private const string FieldName = "speakerId";

    public bool CanHandle(string query)
        => query.StartsWith(SpeakerSpecifier, StringComparison.OrdinalIgnoreCase);

    public FilterOption BuildFilterOption(string filter)
    {
        var fieldValue = filter.Substring(SpeakerSpecifier.Length);

        // There might be several values for the filter
        var values = fieldValue.Split(',');

        return new FilterOption
        {
            FieldName = FieldName,
            Values = values,
            Operator = FilterOperation.Is
        };
    }

    public IEnumerable<IndexFieldValue> GetFieldValues(IContent content, string? culture)
    {
        if (content.ContentType.Alias != "session")
        {
            return [];
        }

        var speakersString = content.GetValue<string>("speakers");

        if (speakersString is null)
        {
            return [];
        }

        var fieldValues = new List<object>();

        foreach (var speakerString in speakersString.Split(','))
        {
            if (speakerString.TryConvertTo<GuidUdi>().Result is { } speakerId)
            {
                fieldValues.Add(speakerId.Guid);
            }
        }

        return [
            new IndexFieldValue
            {
                FieldName = FieldName,
                Values = fieldValues,
            }
        ];
    }

    public IEnumerable<IndexField> GetFields() =>
    [
        new()
        {
            FieldName = FieldName,
            FieldType = FieldType.StringRaw,
            VariesByCulture = false
        }
    ];
}
