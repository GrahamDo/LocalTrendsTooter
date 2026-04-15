namespace LocalTrendsTooter;

internal class TrendsBuilder
{
    public List<Trend> BuildTrends(List<MastodonPost> posts, int maxTagsPerPost)
    {
        var results = new List<Trend>();
        var orderedPosts = posts.OrderBy(p => p.CreatedAt).ToList();
        //Start at the earliest post so LastUsed is always the last used for each hashtag
        orderedPosts.ForEach(post => ProcessPost(post, maxTagsPerPost, results));
        return results.OrderByDescending(t => t.Rank).ThenByDescending(t => t.LastUsed).ToList();
    }

    private void ProcessPost(MastodonPost post, int maxTagsPerPost, List<Trend> results)
    {
        if (post.Tags.Count == 0)
            return;

        var tagLimit = post.Tags.Count < maxTagsPerPost ? post.Tags.Count : maxTagsPerPost;

        for (var i = 0; i < tagLimit; i++)
        {
            var tag = post.Tags[i];

            var trend = results.FirstOrDefault(t => t.HashTag == tag.Name);
            if (trend != null)
            {
                trend.LastUsed = post.CreatedAt;
                trend.Rank += CalculateRank(post);
            }
            else
            {
                results.Add(new Trend
                {
                    HashTag = tag.Name,
                    LastUsed = post.CreatedAt,
                    Rank = CalculateRank(post)
                });
            }
        }
    }

    private static uint CalculateRank(MastodonPost post) => 1 + post.Favourites + (2 * post.Boosts) + (2 * post.Favourites);
}