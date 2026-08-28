var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<LinkStore>();

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
    var existingUrl = store.Get(code);
    if (existingUrl is null)
    {
        return Results.NotFound();
    }
    return Results.Redirect(existingUrl.BaseUrl);
});
app.MapGet("/api/{code}", (string code) => $"Not yet implemented. Code: {code}");

app.Run();
