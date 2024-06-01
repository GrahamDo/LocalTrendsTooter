using Newtonsoft.Json;

namespace LocalTrendsTooter;

internal class MastodonPost
{
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    public List<MastodonTag> Tags { get; set; } = [];
    public string Id { get; set; } = string.Empty;
}