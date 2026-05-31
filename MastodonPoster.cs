using Newtonsoft.Json;
using System.Net;

namespace LocalTrendsTooter;

internal class MastodonPoster(MaxCharactersCacheManager maxCharactersCacheManager)
{
    private static readonly HttpClient Client = new();

    public async Task PostDirect(string instanceUrl, string instanceToken, string message) =>
        await Post(instanceUrl, instanceToken, message, true);
    
    public async Task PostPublic(string instanceUrl, string instanceToken, string message) =>
        await Post(instanceUrl, instanceToken, message, false);

    private async Task Post(string instanceUrl, string instanceToken, string message, bool direct, bool isRetry = false)
    {
        var characterLimit = await GetTootCharacterLimit(instanceUrl, instanceToken);
        var status = new MastodonStatus { Status = ShortenText(message, characterLimit) };
        if (direct)
            status.Visibility = "direct";

        var json = JsonConvert.SerializeObject(status);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, $"{instanceUrl}/api/v1/statuses")
        {
            Content = content,
            Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", instanceToken) }
        };

        using var response = await Client.SendAsync(request);
            
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                throw new ApplicationException("Invalid Mastodon token");
                
            if (response.StatusCode == HttpStatusCode.UnprocessableEntity && !isRetry)
            {
                // It could be the character limit that's decreased. Clear the cache and try again once
                maxCharactersCacheManager.ClearCache();
                await Post(instanceUrl, instanceToken, message, direct, true);
                return;
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }
    }

    private static string ShortenText(string message, int characterLimit)
        => message.Length <= characterLimit ? message : message[..characterLimit];

    private async Task<int> GetTootCharacterLimit(string instanceUrl, string instanceToken)
    {
        var charLimit = maxCharactersCacheManager.GetMaxCharacters();
        if (charLimit > 0)
            return charLimit;

        var request = new HttpRequestMessage(HttpMethod.Get, $"{instanceUrl}/api/v1/instance")
        {
            Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", instanceToken) }
        };

        using var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(content))
            throw new ApplicationException("Empty response from getting instance info");

        var results = JsonConvert.DeserializeObject<InstanceInfo>(content) ??
                      throw new ApplicationException("Can't deserialise instance info");

        charLimit = results.Configuration.Statuses.MaxChars;
        maxCharactersCacheManager.SaveMaxCharacters(charLimit);
        return charLimit;
    }
}