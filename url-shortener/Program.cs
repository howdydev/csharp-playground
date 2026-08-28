var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<LinkStore>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/shorten", (ShortenRequest request, LinkStore store) =>
{
    var generatedCode = store.Add(request.Url);
    return $"Generated code: ${generatedCode}";
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
