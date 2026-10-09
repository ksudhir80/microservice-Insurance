namespace AutoQuoteService.Middleware;

public class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(
        RequestDelegate next,
        ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId =
            context.Request.Headers[HeaderName].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        context.Items[HeaderName] = correlationId;

        context.Response.Headers[HeaderName] = correlationId;

        using (_logger.BeginScope(
            new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
        {
            _logger.LogInformation(
                "Request started: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            _logger.LogInformation(
                "Request completed with status code {StatusCode}",
                context.Response.StatusCode);
        }
    }
}