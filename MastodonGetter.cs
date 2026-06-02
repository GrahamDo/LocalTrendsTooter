using System.Text.Json;

namespace LocalTrendsTooter;

internal class MastodonGetter
{
    private static readonly HttpClient Client = new();

    public async Task<IEnumerable<MastodonPost>> GetPostsWithHashtags(string instanceUrl, DateTime oldestDateUtc)
    {
        const int pageSize = 40;
        var allPosts = new List<MastodonPost>();
        var apiUrl = $"{instanceUrl}/api/v1/timelines/public?local=true&with_replies=true&limit={pageSize}";

        var maxId = string.Empty;
        var done = false;
        while (!done)
        {
            var url = apiUrl;
            if (maxId != string.Empty)
                url += $"&max_id={maxId}";

            var response = await Client.GetStringAsync(url);
            var posts = JsonSerializer.Deserialize<List<MastodonPost>>(response) ??
                        throw new InvalidOperationException($"Failed to retrieve posts from {instanceUrl}");

            posts.RemoveAll(post => post.CreatedAt < oldestDateUtc);
            done = posts.Count < pageSize;
            if (!done)
                maxId = posts[^1].Id;

            posts.RemoveAll(post => post.Tags.Count == 0);
            allPosts.AddRange(posts);
        }

        return allPosts;
    }
}