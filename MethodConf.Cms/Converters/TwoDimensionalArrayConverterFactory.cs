using System.Text.Json;
using System.Text.Json.Serialization;

namespace MethodConf.Cms.Converters;

public class TwoDimensionalArrayConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsArray && typeToConvert.GetArrayRank() == 2;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetElementType();

        if (elementType is null)
        {
            return null;
        }

        var converterType = typeof(TwoDimensionalArrayConverter<>).MakeGenericType(elementType);

        return Activator.CreateInstance(converterType) as JsonConverter;
    }
}
