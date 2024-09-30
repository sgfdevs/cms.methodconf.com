using System.Text.Json;
using MethodConf.Cms.Converters;
using MethodConf.Cms.Tests.Utilities;

namespace MethodConf.Cms.Tests;

[TestFixture]
public class TwoDimensionalArrayConverterTests
{
    private JsonSerializerOptions _options;

    [SetUp]
    public void Setup()
    {
        _options = new JsonSerializerOptions
        {
            Converters = { new TwoDimensionalArrayConverterFactory() },
            WriteIndented = false
        };
    }

    [Test]
    public void TestIntArraySerialization()
    {
        var array = new[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        const string expectedJson = "[[1,2,3],[4,5,6]]";

        var json = JsonSerializer.Serialize(array, _options);
        Assert.That(json, Is.EqualTo(expectedJson));

        var deserializedArray = JsonSerializer.Deserialize<int[,]>(json, _options);
        GridAssert.ThatGridsAreEqual(array, deserializedArray);
    }

    [Test]
    public void TestStringArraySerialization()
    {
        var array = new[,]
        {
            { "A", "B", "C" },
            { "D", "E", "F" }
        };

        const string expectedJson = """[["A","B","C"],["D","E","F"]]""";

        var json = JsonSerializer.Serialize(array, _options);
        Assert.That(json, Is.EqualTo(expectedJson));

        var deserializedArray = JsonSerializer.Deserialize<string[,]>(json, _options);
        GridAssert.ThatGridsAreEqual(array, deserializedArray);
    }

    [Test]
    public void TestNullableIntArraySerialization()
    {
        var array = new int?[,]
        {
            { 1, null, 3 },
            { null, 5, null }
        };

        const string expectedJson = "[[1,null,3],[null,5,null]]";

        var json = JsonSerializer.Serialize(array, _options);
        Assert.That(json, Is.EqualTo(expectedJson));

        var deserializedArray = JsonSerializer.Deserialize<int?[,]>(json, _options);
        GridAssert.ThatGridsAreEqual(array, deserializedArray);
    }

    [Test]
    public void TestComplexTypeArraySerialization()
    {
        var array = new[,]
        {
            {
                new MyClass { Id = 1, Name = "Alice" },
                new MyClass { Id = 2, Name = "Bob" }
            },
            {
                new MyClass { Id = 3, Name = "Charlie" },
                null
            }
        };

        const string expectedJson = """[[{"Id":1,"Name":"Alice"},{"Id":2,"Name":"Bob"}],[{"Id":3,"Name":"Charlie"},null]]""";

        var json = JsonSerializer.Serialize(array, _options);
        Assert.That(json, Is.EqualTo(expectedJson));

        var deserializedArray = JsonSerializer.Deserialize<MyClass[,]>(json, _options);
        GridAssert.ThatGridsAreEqual(array, deserializedArray);
    }

    [Test]
    public void TestDeserializationWithMismatchedRowLengths()
    {
        const string invalidJson = "[[1,2,3],[4,5]]"; // Second row has fewer elements

        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<int[,]>(invalidJson, _options);
        });
    }

    [Test]
    public void TestDeserializationWithInvalidJson()
    {
        const string invalidJson = """{"invalid":"json"}""";

        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<int[,]>(invalidJson, _options);
        });
    }

    public class MyClass
    {
        public int Id { get; init; }
        public required string Name { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is MyClass other)
            {
                return Id == other.Id && Name == other.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }

}
