using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fitonyashka.InfrastructureLayer;

public static class JsonSerializationOptions
{
    public static readonly JsonSerializerOptions Default = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters =
        {
            new DateOnlyJsonConverter(),
            new TimeOnlyJsonConverter()
        }
    };
}

public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.Parse(reader.GetString() ?? throw new JsonException("Invalid DateOnly value."));
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}

public sealed class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeOnly.Parse(reader.GetString() ?? throw new JsonException("Invalid TimeOnly value."));
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("HH:mm:ss"));
    }
}
