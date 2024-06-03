namespace LocalTrendsTooter;

internal class Trend
{
    public string HashTag { get; set; } = string.Empty;
    public uint Rank { get; set; }
    public DateTime LastUsed { get; set; } = new();
}