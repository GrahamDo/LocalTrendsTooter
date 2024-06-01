namespace LocalTrendsTooter;

internal class MastodonPoster(MaxCharactersCacheManager maxCharactersCacheManager)
{
    public void PostDirect(string instanceUrl, string instanceToken, string message) =>
        Post(instanceUrl, instanceToken, message, true);
    public void PostPublic(string instanceUrl, string instanceToken, string message) =>
        Post(instanceUrl, instanceToken, message, false);

    private static void Post(string instanceUrl, string instanceToken, string message, bool direct)
    {
    }
}