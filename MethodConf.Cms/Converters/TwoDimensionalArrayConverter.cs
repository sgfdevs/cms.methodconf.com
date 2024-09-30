using System.Text.Json;
using System.Text.Json.Serialization;

namespace MethodConf.Cms.Converters;

public class TwoDimensionalArrayConverter<T> : JsonConverter<T[,]>
{
    public override void Write(Utf8JsonWriter writer, T[,] value, JsonSerializerOptions options)
    {
        var rows = value.GetLength(0);
        var cols = value.GetLength(1);

        writer.WriteStartArray();
        for (var i = 0; i < rows; i++)
        {
            writer.WriteStartArray();
            for (var j = 0; j < cols; j++)
            {
                JsonSerializer.Serialize(writer, value[i, j], options);
            }
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }

    public override T[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Check if the token is the start of an array
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected start of array.");
        }

        var rows = new List<List<T>>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected start of inner array.");
            }

            var row = new List<T>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                T value = JsonSerializer.Deserialize<T>(ref reader, options)!;
                row.Add(value);
            }

            rows.Add(row);
        }

        var numRows = rows.Count;
        var numCols = numRows > 0 ? rows[0].Count : 0;

        if (rows.Any(row => row.Count != numCols))
        {
            throw new JsonException("All rows must have the same number of columns.");
        }

        var result = new T[numRows, numCols];
        for (var i = 0; i < numRows; i++)
        {
            for (var j = 0; j < numCols; j++)
            {
                result[i, j] = rows[i][j]!;
            }
        }

        return result;
    }
}
