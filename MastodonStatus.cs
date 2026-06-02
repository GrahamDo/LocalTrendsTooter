using Newtonsoft.Json;

namespace LocalTrendsTooter;

internal class MastodonStatus
{
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
    [JsonProperty("visibility")]
    public string Visibility { get; set; } = "public";
}