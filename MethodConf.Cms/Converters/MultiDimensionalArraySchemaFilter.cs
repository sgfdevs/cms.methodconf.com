using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MethodConf.Cms.Converters;

public class MultiDimensionalArraySchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;

        if (!type.IsArray || type.GetArrayRank() <= 1)
        {
            return;
        }

        var elementType = type.GetElementType();

        var elementSchema = context.SchemaGenerator.GenerateSchema(elementType, context.SchemaRepository);

        for (var i = type.GetArrayRank(); i > 0; i--)
        {
            elementSchema = new OpenApiSchema
            {
                Type = "array",
                Items = elementSchema
            };
        }

        schema.Type = "array";
        schema.Items = elementSchema.Items;
    }
}
