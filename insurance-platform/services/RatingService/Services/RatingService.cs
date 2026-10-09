using RatingService.Models;

namespace RatingService.Services;

public class RatingService : IRatingService
{
    public Task<RatingResponse> CalculateAutoRatingAsync(
        RatingRequest request)
    {
        decimal basePremium = 1200m;

        decimal riskAdjustment = 0m;

        // Accident surcharge
        riskAdjustment += request.Accidents * 150m;

        // Violation surcharge
        riskAdjustment += request.Violations * 75m;

        // Younger driver surcharge
        if (request.DriverAge < 25)
        {
            riskAdjustment += 300m;
        }

        // Older vehicle adjustment
        if (request.VehicleYear < 2018)
        {
            riskAdjustment += 100m;
        }

        decimal coverageAdjustment = request.CoverageLevel
            .ToLowerInvariant() switch
        {
            "basic" => 0m,
            "standard" => 100m,
            "full" => 225m,
            _ => 225m
        };

        decimal finalPremium =
            basePremium +
            riskAdjustment +
            coverageAdjustment;

        return Task.FromResult(new RatingResponse
        {
            BasePremium = basePremium,
            RiskAdjustment = riskAdjustment,
            CoverageAdjustment = coverageAdjustment,
            FinalPremium = finalPremium
        });
    }
}