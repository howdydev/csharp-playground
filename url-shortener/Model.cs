using Microsoft.EntityFrameworkCore;

public class LinkContext : DbContext
{
    public DbSet<Link> Links { get; set; }

    public string DbPath { get; }

    public LinkContext(DbContextOptions<LinkContext> options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "links.db");
    }
}

[Index(nameof(Code), IsUnique = true)]
public class Link
{
    public int LinkId { get; set; }
    public required string BaseUrl { get; set; }
    public required string Code { get; set; }
    public DateTime CreatedAt { get; set; }
}
