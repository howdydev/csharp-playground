using System.Text;

public record GeneratedUrl(string BaseUrl, DateTime GeneratedAt);
public record AddResult(string Code, AddStatusCode Status);

public enum AddStatusCode
{
    Success,
    InvalidUrl,
}

public class LinkStore
{
    private Dictionary<string, GeneratedUrl> generatedUrls = new();

    public AddResult Add(string url)
    {
        if (!ValidateUrl(url, out var _uriRequest)) 
            return new AddResult(string.Empty, AddStatusCode.InvalidUrl);

        var existingEntry = generatedUrls.FirstOrDefault(entry => entry.Value.BaseUrl == url);

        if (!existingEntry.Equals(default(KeyValuePair<string, GeneratedUrl>)))
        {
            return new AddResult(existingEntry.Key, AddStatusCode.Success);
        }

        var code = GenerateCode();
        GeneratedUrl generatedUrl = new(url, DateTime.UtcNow);
        generatedUrls.Add(code, generatedUrl);
        return new AddResult(code, AddStatusCode.Success);
    }

    public GeneratedUrl? Get(string url)
    {
        if (generatedUrls.TryGetValue(url, out var existing))
        {
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

    private static bool ValidateUrl(string url, out Uri? uriResult)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out uriResult))
        {
            return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
        }

        return false;
    }
}
