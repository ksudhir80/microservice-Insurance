using RatingService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllers();

builder.Services.AddScoped<IRatingService, RatingService.Services.RatingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();