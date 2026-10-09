namespace AutoQuoteService.Models;

public class RatingResponse
{
    public decimal BasePremium { get; set; }

    public decimal RiskAdjustment { get; set; }

    public decimal CoverageAdjustment { get; set; }

    public decimal FinalPremium { get; set; }
}