using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MethodConf.Cms.Converters;

public class MultiDimensionalArraySchemaFilter : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        if (!type.IsArray || type.GetArrayRank() <= 1)
        {
            return Task.CompletedTask;
        }

        var elementType = type.GetElementType()!;
        var elementSchema = CreateElementSchema(elementType);

        for (var i = type.GetArrayRank(); i > 0; i--)
        {
            elementSchema = new OpenApiSchema
            {
                Type = JsonSchemaType.Array,
                Items = elementSchema,
            };
        }

        schema.Type = JsonSchemaType.Array;
        schema.Items = elementSchema.Items;

        return Task.CompletedTask;
    }

    private static OpenApiSchema CreateElementSchema(Type elementType) =>
        elementType == typeof(string)
            ? new OpenApiSchema { Type = JsonSchemaType.String }
            : new OpenApiSchema();
}
