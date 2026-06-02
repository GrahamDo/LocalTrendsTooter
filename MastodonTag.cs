using System.Text.Json.Serialization;

namespace LocalTrendsTooter;

internal class MastodonTag
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}