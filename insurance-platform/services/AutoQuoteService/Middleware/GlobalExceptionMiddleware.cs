using Polly.Timeout;
using System.Text.Json;

namespace AutoQuoteService.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (TimeoutRejectedException ex)
        {
            await HandleExceptionAsync(
                context,
                ex,
                StatusCodes.Status503ServiceUnavailable,
                "Rating service is temporarily unavailable.");
        }
        catch (HttpRequestException ex)
        {
            await HandleExceptionAsync(
                context,
                ex,
                StatusCodes.Status503ServiceUnavailable,
                "Rating service is temporarily unavailable.");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex,
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        int statusCode,
        string message)
    {
        var correlationId =
            context.Items["X-Correlation-ID"]?.ToString()
            ?? Guid.NewGuid().ToString();

        _logger.LogError(
            exception,
            "Request failed. CorrelationId: {CorrelationId}",
            correlationId);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message,
            correlationId
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}