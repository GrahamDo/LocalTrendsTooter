using Newtonsoft.Json;

namespace LocalTrendsTooter;

internal class Settings
{
    private const string SettingsFileName = "settings.json";

    public int HistoryHours { get; set; } = 1;
    public List<string> InstancesToTrend { get; set; } = [];
    public string PostInstance { get; set; } = string.Empty;
    public string PostInstanceToken { get; set; } = string.Empty;
    public string DmAccountName { get; set; } = string.Empty;

    public static Settings Load()
    {
        if (!File.Exists(SettingsFileName))
            return new Settings();

        var text = File.ReadAllText(SettingsFileName);
        return JsonConvert.DeserializeObject<Settings>(text) ??
               throw new ApplicationException($"Your '{SettingsFileName}' appears to be empty or corrupt.");
    }

    public void SetValueFromArguments(string name, string value)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
}