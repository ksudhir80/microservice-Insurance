namespace AutoQuoteService.Models;

public class AutoQuoteResponse
{
    public string QuoteId { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public decimal BasePremium { get; set; }

    public decimal RiskAdjustment { get; set; }

    public decimal CoverageAdjustment { get; set; }

    public decimal Premium { get; set; }

    public decimal MonthlyPremium { get; set; }

    public string Status { get; set; } = string.Empty;
}