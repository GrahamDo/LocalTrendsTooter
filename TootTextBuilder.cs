using System.Text;

namespace LocalTrendsTooter;

internal class TootTextBuilder
{
    private const string TemplateFileName = "toot-template.txt";

    public string Build(List<Trend> trends, int top)
    {
        var templateText = LoadFromFile();
        var trendsText = GetTrendsText(trends, top);
        return templateText.Replace("{trends}", trendsText);
    }

    private static string GetTrendsText(List<Trend> trends, int top)
    {
        var results = new StringBuilder(top);
        foreach (var trend in trends.Take(top))
            results.AppendLine($"#{trend.HashTag}");

        return results.ToString();
    }

    private static string LoadFromFile()
    {
        if (!File.Exists(TemplateFileName))
            throw new ApplicationException($"{TemplateFileName} not found");

        var text = File.ReadAllText(TemplateFileName);
        return text;
    }
}