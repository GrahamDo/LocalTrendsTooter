namespace LocalTrendsTooter;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var settings = Settings.Load();
        var mastodonPoster = new MastodonPoster(new MaxCharactersCacheManager());

        try
        {
            if (args.Length == 3 && args[0].ToLower() == "--set")
            {
                settings.SetValueFromArguments(args[1], args[2]);
                settings.Save();
                return;
            }

            var historyHours = settings.HistoryHours;
            var oldestDateUtc = DateTime.UtcNow.AddHours(historyHours * -1);
            var mastodonGetter = new MastodonGetter();
            var instancesToTrend = settings.InstancesToTrend;
            var allPosts = new List<MastodonPost>();
            foreach (var instance in instancesToTrend)
            {
                try
                {
                    var posts = await mastodonGetter.GetPostsWithHashtags(instance, oldestDateUtc);
                    allPosts.AddRange(posts);
                }
                catch (Exception ex)
                {
                    var message = $"Error fetching from {instance}:\r\n{ex}";
                    await ReportError(mastodonPoster, settings, message);
                }
            }

            allPosts.RemoveAll(post => post.Account.Url == settings.ExcludeAccountUrl);
            if (!allPosts.Any() && !string.IsNullOrEmpty(settings.DmAccountName))
            {
                await mastodonPoster.PostDirect(settings.PostInstance, settings.PostInstanceToken,
                    $"{settings.DmAccountName} No recent posts found. Sent DM to {settings.DmAccountName}");
                return;
            }

            var trendsBuilder = new TrendsBuilder();
            var trends = trendsBuilder.BuildTrends(allPosts);
            var tootTextBuilder = new TootTextBuilder();
            var tootText = tootTextBuilder.Build(trends, settings.Top);
            await mastodonPoster.PostPublic(settings.PostInstance, settings.PostInstanceToken, tootText);
        }
        catch (ApplicationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            await ReportError(mastodonPoster, settings, ex.ToString());
        }
    }

    private static async Task ReportError(MastodonPoster mastodonPoster, Settings settings, string message)
    {
        Console.WriteLine(message);
        if (string.IsNullOrEmpty(settings.DmAccountName)) 
            return;
        
        try
        {
            await mastodonPoster.PostDirect(settings.PostInstance, settings.PostInstanceToken,
                $"{settings.DmAccountName} {message}");
            Console.WriteLine($"Sent DM to {settings.DmAccountName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"Failed to send DM to {settings.DmAccountName}\r\n{ex}");
        }
    }
}