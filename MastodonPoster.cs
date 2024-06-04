using Newtonsoft.Json;

namespace LocalTrendsTooter;

internal class MastodonPoster(MaxCharactersCacheManager maxCharactersCacheManager)
{
    public async Task PostDirect(string instanceUrl, string instanceToken, string message) =>
        await Post(instanceUrl, instanceToken, message, true);
    public async Task PostPublic(string instanceUrl, string instanceToken, string message) =>
        await Post(instanceUrl, instanceToken, message, false);

    private async Task Post(string instanceUrl, string instanceToken, string message, bool direct)
    {
        var client = new HttpClient();
        var characterLimit = await GetTootCharacterLimit(client, instanceUrl);
    }

    private async Task<int> GetTootCharacterLimit(HttpClient client, string instanceUrl)
    {
        var charLimit = maxCharactersCacheManager.GetMaxCharacters();
        if (charLimit > 0)
            return charLimit;

        // This assumes the client has been initialised by the Post method
        var response = await client.GetStringAsync($"{instanceUrl}/api/v1/instance");
        var results = JsonConvert.DeserializeObject<InstanceInfo>(response) ??
            throw new ApplicationException("Can't deserialise instance info");

        charLimit = results.Configuration.Statuses.MaxChars;
        maxCharactersCacheManager.SaveMaxCharacters(charLimit);
        return charLimit;
    }
}