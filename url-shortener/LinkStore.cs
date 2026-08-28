using System.Text;

public record GeneratedUrl(string BaseUrl, DateTime GeneratedAt);

public class LinkStore
{
    private Dictionary<string, GeneratedUrl> generatedUrls = new();

    public string Add(string url)
    {
        var existingEntry = generatedUrls.FirstOrDefault(entry => entry.Value.BaseUrl == url);

        if (!existingEntry.Equals(default(KeyValuePair<string, GeneratedUrl>)))
        {
            return existingEntry.Key;
        }

        var code = GenerateCode();
        GeneratedUrl generatedUrl = new(url, DateTime.UtcNow);
        generatedUrls.Add(code, generatedUrl);
        return code;
    }

    public GeneratedUrl? Get(string url) {
        if (generatedUrls.TryGetValue(url, out var existing)) {
            return existing;
        }

        return null;
    }

    private string GenerateCode()
    {
        int stringLen = 9;
        StringBuilder newCode = new("");

        for (int i = 0; i < stringLen; i++)
        {
            var letter = RandomChar();
            newCode.Append(letter);
        }

        var code = newCode.ToString();
        if (generatedUrls.ContainsKey(code)) return GenerateCode();

        return code;
    }

    private static char RandomChar()
    {
        var value = Random.Shared.Next(62);

        if (value < 10) return (char)('0' + value);
        if (value < 36) return (char)('A' + value - 10);
        return (char)('a' + value - 36);
    }
}
