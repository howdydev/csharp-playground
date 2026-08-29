using System.Text;

public record GeneratedUrl(string BaseUrl, DateTime GeneratedAt);
public record AddResult(string Code, AddStatusCode Status);

public enum AddStatusCode
{
    Success,
    InvalidUrl,
}

public class LinkStore(LinkContext db)
{
    public AddResult Add(string url)
    {
        if (!ValidateUrl(url, out _))
            return new AddResult(string.Empty, AddStatusCode.InvalidUrl);

        var code = GenerateCode();
        Link generatedUrl = new()
        {
            BaseUrl = url,
            Code = code,
            CreatedAt = DateTime.UtcNow,
        };
        db.Links.Add(generatedUrl);
        db.SaveChanges();
        return new AddResult(code, AddStatusCode.Success);
    }

    public Link? Get(string code)
    {
        var existing = db.Links.FirstOrDefault(l => l.Code == code);
        return existing;
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
        if (db.Links.Any(l => l.Code == code)) return GenerateCode();

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
