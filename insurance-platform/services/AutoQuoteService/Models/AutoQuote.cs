namespace AutoQuoteService.Models;

public class AutoQuote
{
    public int Id { get; set; }

    public string QuoteId { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public string VehicleMake { get; set; } = string.Empty;

    public string VehicleModel { get; set; } = string.Empty;

    public int VehicleYear { get; set; }

    public string Vin { get; set; } = string.Empty;

    public DateOnly DriverDateOfBirth { get; set; }

    public string LicenseState { get; set; } = string.Empty;

    public int Accidents { get; set; }

    public int Violations { get; set; }

    public string BodilyInjury { get; set; } = string.Empty;

    public decimal PropertyDamage { get; set; }

    public bool Collision { get; set; }

    public bool Comprehensive { get; set; }

    public decimal BasePremium { get; set; }

    public decimal RiskAdjustment { get; set; }

    public decimal CoverageAdjustment { get; set; }

    public decimal Premium { get; set; }

    public decimal MonthlyPremium { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}