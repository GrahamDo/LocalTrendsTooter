using Newtonsoft.Json;

namespace LocalTrendsTooter;

internal class InstanceInfo
{
    public InstanceInfoConfiguration Configuration { get; set; } = new();
}

internal class InstanceInfoConfiguration
{
    public InstanceInfoConfigurationStatuses Statuses { get; set; } = new();
}

internal class InstanceInfoConfigurationStatuses
{
    [JsonProperty("max_characters")]
    public int MaxChars { get; set; }
}
