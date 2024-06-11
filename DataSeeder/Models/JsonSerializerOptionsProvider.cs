using System.Text.Json;

namespace DataSeeder.Models;

public class JsonSerializerOptionsProvider
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return _jsonSerializerOptions;
    }
}