namespace LocalTrendsTooter;

internal class Settings
{
    public int HistoryHours { get; set; } = 1;
    public List<string> InstancesToTrend { get; set; } = [];
    public string PostInstance { get; set; } = string.Empty;
    public string PostInstanceToken { get; set; } = string.Empty;
    public string DmAccountName { get; set; } = string.Empty;

    public static Settings Load()
    {
        throw new NotImplementedException();
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