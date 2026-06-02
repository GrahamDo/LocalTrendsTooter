using System.Text.Json.Serialization;

namespace LocalTrendsTooter;

internal class InstanceInfo
{
    [JsonPropertyName("configuration")]
    public InstanceInfoConfiguration Configuration { get; set; } = new();
}

internal class InstanceInfoConfiguration
{
    [JsonPropertyName("statuses")]
    public InstanceInfoConfigurationStatuses Statuses { get; set; } = new();
}

internal class InstanceInfoConfigurationStatuses
{
    [JsonPropertyName("max_characters")]
    public int MaxChars { get; set; }
}
