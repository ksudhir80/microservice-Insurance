namespace AutoQuoteService.Models;

public class CoverageInfo
{
    public string BodilyInjury { get; set; } = string.Empty;

    public decimal PropertyDamage { get; set; }

    public bool Collision { get; set; }

    public bool Comprehensive { get; set; }
}