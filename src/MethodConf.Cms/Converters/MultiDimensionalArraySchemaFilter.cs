using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MethodConf.Cms.Converters;

public class MultiDimensionalArraySchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema openApiSchema)
        {
            return;
        }

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
                Type = JsonSchemaType.Array,
                Items = elementSchema
            };
        }

        openApiSchema.Type = JsonSchemaType.Array;
        openApiSchema.Items = elementSchema.Items;
    }
}
