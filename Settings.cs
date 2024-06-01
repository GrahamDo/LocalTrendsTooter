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
        switch (name.ToLower())
        {
            case "historyhours":
                HistoryHours = TryConvertInt(value);
                break;
            case "instancestotrend":
                var instances = value.Split(',');
                var instancesList = new List<string>();
                foreach (var currencyCode in instances)
                    instancesList.Add(currencyCode);
                InstancesToTrend = instancesList;
                break;
            case "postinstance":
                PostInstance = value;
                break;
            case "postinstancetoken":
                PostInstanceToken = value;
                break;
            case "dmaccountname":
                DmAccountName = value;
                break;
            default:
                throw new ApplicationException($"Invalid setting: {name}");
        }
    }

    private int TryConvertInt(string value)
    {
        var isInt = int.TryParse(value, out var result);
        if (!isInt)
            throw new ApplicationException($"Invalid argument: {value}");
        return result;
    }

    public void Save()
    {
        var serialised = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(SettingsFileName, serialised);
    }
}