using Newtonsoft.Json;

namespace LocalTrendsTooter;

internal class MastodonPost
{
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    public Account Account { get; set; }
    public List<MastodonTag> Tags { get; set; } = [];
    public string Id { get; set; } = string.Empty;
    [JsonProperty("reblogs_count")]
    public uint Boosts { get; set; }
    [JsonProperty("favourites_count")]
    public uint Favourites { get; set; }
    [JsonProperty("replies_count")]
    public uint Replies { get; set; }
}

internal class Account
{
    public string Url { get; set; }
}