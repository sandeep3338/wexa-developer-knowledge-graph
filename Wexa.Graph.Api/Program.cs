using Neo4j.Driver;
using Wexa.Graph.Api.Data;
using Wexa.Graph.Api.Repositories;
using Wexa.Graph.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var uri = builder.Configuration["CognoDB:Uri"];
var username = builder.Configuration["CognoDB:Username"];
var password = builder.Configuration["CognoDB:Password"];

if (string.IsNullOrWhiteSpace(uri) ||
    string.IsNullOrWhiteSpace(username) ||
    string.IsNullOrWhiteSpace(password))
{
    throw new InvalidOperationException(
        "CognoDB connection configuration is missing.");
}

builder.Services.AddSingleton<IDriver>(_ =>
    GraphDatabase.Driver(
        uri,
        AuthTokens.Basic(username, password)));

builder.Services.AddScoped<GraphRepository>();

builder.Services.AddScoped<DeveloperService>();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("Frontend");


app.MapGet("/api/health/graph", async (IDriver driver) =>
{
    try
    {
        await using var session = driver.AsyncSession();

        var result = await session.RunAsync(
            "RETURN 1 AS result");

        var record = await result.SingleAsync();

        return Results.Ok(new
        {
            connected = true,
            result = record["result"].As<int>()
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "CognoDB connection failed",
            detail: ex.Message,
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }
});

app.MapControllers();

app.Run();