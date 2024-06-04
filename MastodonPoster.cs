using Newtonsoft.Json;
using System.Net;
using RestSharp;
using RestSharp.Authenticators.OAuth2;

namespace LocalTrendsTooter;

internal class MastodonPoster(MaxCharactersCacheManager maxCharactersCacheManager)
{
    public async Task PostDirect(string instanceUrl, string instanceToken, string message) =>
        await Post(instanceUrl, instanceToken, message, true);
    public async Task PostPublic(string instanceUrl, string instanceToken, string message) =>
        await Post(instanceUrl, instanceToken, message, false);

    private async Task Post(string instanceUrl, string instanceToken, string message, bool direct, bool isRetry = false)
    {
        var restOptions = new RestClientOptions($"{instanceUrl}/api/v1/")
        {
            Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(instanceToken, "Bearer")
        };
        var client = new RestClient(restOptions);
        
        var characterLimit = await GetTootCharacterLimit(client);
        var status = new MastodonStatus { Status = ShortenText(message, characterLimit) };
        if (direct)
            status.Visibility = "direct";

        var request = new RestRequest("statuses", Method.Post).AddJsonBody(status);
        try
        {
            await client.PostAsync(request);
        }
        catch (HttpRequestException ex)
        {
            if (ex.Message.Contains("Forbidden") || ex.Message.Contains("Unauthorized"))
                throw new ApplicationException("Invalid Mastodon token");
            if (ex.StatusCode == HttpStatusCode.UnprocessableEntity && !isRetry)
            {
                // It could be the character limit that's decreased. Clear the cache and try again once
                maxCharactersCacheManager.ClearCache();
                await Post(instanceUrl, instanceToken, message, direct, true);
                return;
            }

            throw;
        }
    }

    private static string ShortenText(string message, int characterLimit)
        => message.Length <= characterLimit ? message : message[..characterLimit];

    private async Task<int> GetTootCharacterLimit(RestClient client)
    {
        var charLimit = maxCharactersCacheManager.GetMaxCharacters();
        if (charLimit > 0)
            return charLimit;

        var request = new RestRequest("instance");
        var response = await client.GetAsync(request);
        if (response?.Content == null)
            throw new ApplicationException("Empty response from getting instance info");
        
        var results = JsonConvert.DeserializeObject<InstanceInfo>(response.Content) ??
            throw new ApplicationException("Can't deserialise instance info");

        charLimit = results.Configuration.Statuses.MaxChars;
        maxCharactersCacheManager.SaveMaxCharacters(charLimit);
        return charLimit;
    }
}