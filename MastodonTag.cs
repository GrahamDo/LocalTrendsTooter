using System.Text.Json.Serialization;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// All properties must be read/write for serialization

namespace LocalTrendsTooter;

internal class MastodonTag
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}