using Datadog.Trace;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

// Simple trace test
app.MapGet("/api/test", async () =>
{
    using var scope = Tracer.Instance.StartActive("custom.test-operation");
    scope.Span.SetTag("test.source", "manual-span");

    await Task.Delay(50); // simulate work

    return Results.Ok(new { message = "trace generated", traceId = scope.Span.TraceId.ToString() });
});

// Simulate slow endpoint
app.MapGet("/api/slow", async () =>
{
    using var scope = Tracer.Instance.StartActive("custom.slow-operation");
    scope.Span.SetTag("operation.type", "slow");

    await Task.Delay(Random.Shared.Next(200, 800));

    return Results.Ok(new { message = "slow response done" });
});

// Simulate error for Datadog error tracking
app.MapGet("/api/error", () =>
{
    throw new InvalidOperationException("This is a test error for Datadog.");
});

app.Run();
