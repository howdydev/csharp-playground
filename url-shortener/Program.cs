using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
var dbPath = Path.Join(path, "links.db");
builder.Services.AddDbContext<LinkContext>(options => options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddScoped<LinkStore>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/shorten", (ShortenRequest request, LinkStore store, HttpContext ctx) =>
{
    var generation = store.Add(request.Url);
    return generation.Status switch
    {
        AddStatusCode.Success => Results.Ok($"{ctx.Request.Scheme}://{ctx.Request.Host}/{generation.Code}"),
        AddStatusCode.InvalidUrl => Results.BadRequest("Invalid url supplied."),
        _ => Results.BadRequest("Unknown request"),
    };
});
app.MapGet("/{code}", (string code, LinkStore store) =>
{
    var link = store.RecordClick(code);
    if (link is null) return Results.NotFound();
    return Results.Redirect(link.BaseUrl);
});
app.MapGet("/api/{code}", (string code, LinkStore store) =>
{
    var link = store.Get(code);
    if (link is null) return Results.NotFound();
    return Results.Ok(LinkResponse.From(link));
});

app.Run();
