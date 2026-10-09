using AutoQuoteService.Data;
using AutoQuoteService.Middleware;
using AutoQuoteService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllers();

builder.Services.AddDbContext<QuoteDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("InsuranceDb")
    ));

builder.Services
  .AddHttpClient<IQuoteService, QuoteService>(client =>
{
    client.BaseAddress =
        new Uri(
            builder.Configuration["Services:RatingServiceUrl"]
            ?? "http://localhost:5003/");
})
    .AddResilienceHandler(
        "RatingServiceResilience",
        pipeline =>
        {
            pipeline.AddRetry(
                new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay =
                        TimeSpan.FromMilliseconds(500),
                    BackoffType =
                        DelayBackoffType.Exponential,
                    UseJitter = true
                });

            pipeline.AddCircuitBreaker(
                new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration =
                        TimeSpan.FromSeconds(10),
                    MinimumThroughput = 5,
                    BreakDuration =
                        TimeSpan.FromSeconds(15)
                });

            pipeline.AddTimeout(
                TimeSpan.FromSeconds(3));
        });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANT: middleware order
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();