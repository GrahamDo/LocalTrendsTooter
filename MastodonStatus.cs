using System.Text.Json.Serialization;

namespace LocalTrendsTooter;

internal class MastodonStatus
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    [JsonPropertyName("visibility")]
    public string Visibility { get; set; } = "public";
}