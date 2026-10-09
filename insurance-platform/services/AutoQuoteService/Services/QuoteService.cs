using AutoQuoteService.Data;
using AutoQuoteService.Models;
using System.Net.Http.Json;


namespace AutoQuoteService.Services;

public class QuoteService : IQuoteService
{
    private readonly HttpClient _httpClient;
private readonly QuoteDbContext _dbContext;

public QuoteService(
    HttpClient httpClient,
    QuoteDbContext dbContext)
{
    _httpClient = httpClient;
    _dbContext = dbContext;
}

    public async Task<AutoQuoteResponse> CreateAutoQuoteAsync(
        AutoQuoteRequest request)
    {
        // Calculate driver age
        var driverAge = CalculateAge(
            request.Driver.DateOfBirth);

        // Build request for RatingService
        var ratingRequest = new
        {
            vehicleYear = request.Vehicle.Year,
            driverAge = driverAge,
            accidents = request.Driver.Accidents,
            violations = request.Driver.Violations,
            coverageLevel = GetCoverageLevel(request)
        };

        // Call RatingService
        var ratingResponse =
            await _httpClient.PostAsJsonAsync(
                "api/rating/auto",
                ratingRequest);

        if (!ratingResponse.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"RatingService returned " +
                $"{(int)ratingResponse.StatusCode}.");
        }

        var rating =
            await ratingResponse.Content
                .ReadFromJsonAsync<RatingResponse>();

        if (rating == null)
        {
            throw new InvalidOperationException(
                "RatingService returned an empty response.");
        }

        // Build quote response
        var response = new AutoQuoteResponse
{
    QuoteId = $"QT-{Random.Shared.Next(100000, 999999)}",
    CustomerId = request.CustomerId,
    BasePremium = rating.BasePremium,
    RiskAdjustment = rating.RiskAdjustment,
    CoverageAdjustment = rating.CoverageAdjustment,
    Premium = rating.FinalPremium,
    MonthlyPremium = Math.Round(
        rating.FinalPremium / 12, 2),
    Status = "Quoted"
};


var quote = new AutoQuote
{
    QuoteId = response.QuoteId,
    CustomerId = request.CustomerId,

    VehicleMake = request.Vehicle.Make,
    VehicleModel = request.Vehicle.Model,
    VehicleYear = request.Vehicle.Year,
    Vin = request.Vehicle.Vin,

    DriverDateOfBirth =
        request.Driver.DateOfBirth,

    LicenseState =
        request.Driver.LicenseState,

    Accidents =
        request.Driver.Accidents,

    Violations =
        request.Driver.Violations,

    BodilyInjury =
        request.Coverage.BodilyInjury,

    PropertyDamage =
        request.Coverage.PropertyDamage,

    Collision =
        request.Coverage.Collision,

    Comprehensive =
        request.Coverage.Comprehensive,

    BasePremium =
        response.BasePremium,

    RiskAdjustment =
        response.RiskAdjustment,

    CoverageAdjustment =
        response.CoverageAdjustment,

    Premium =
        response.Premium,

    MonthlyPremium =
        response.MonthlyPremium,

    Status =
        response.Status,

    CreatedAt =
        DateTime.UtcNow
};

_dbContext.AutoQuotes.Add(quote);

await _dbContext.SaveChangesAsync();

return response;
     
    }

    private static int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(
            DateTime.UtcNow);

        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    private static string GetCoverageLevel(
        AutoQuoteRequest request)
    {
        if (request.Coverage.Collision &&
            request.Coverage.Comprehensive)
        {
            return "Full";
        }

        if (request.Coverage.Collision ||
            request.Coverage.Comprehensive)
        {
            return "Standard";
        }

        return "Basic";
    }
}