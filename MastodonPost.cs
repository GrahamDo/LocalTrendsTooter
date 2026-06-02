using System.Text.Json.Serialization;

namespace LocalTrendsTooter;

internal class MastodonPost
{
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("account")]
    public required Account Account { get; set; }
    [JsonPropertyName("tags")]
    public List<MastodonTag> Tags { get; set; } = [];
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("reblogs_count")]
    public uint Boosts { get; set; }
    [JsonPropertyName("favourites_count")]
    public uint Favourites { get; set; }
    [JsonPropertyName("replies_count")]
    public uint Replies { get; set; }
}

internal class Account
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }
    [JsonPropertyName("bot")]
    public bool Bot { get; set; }
}