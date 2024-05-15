var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/sync", () =>
{
    Thread.Sleep(1000);
    return Random.Shared.Next(100);
});

app.MapGet("/async", async () =>
{
    await Task.Delay(1000);
    return Random.Shared.Next(100);
});

app.Run();