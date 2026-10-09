namespace RatingService.Models;

public class RatingRequest
{
    public int VehicleYear { get; set; }

    public int DriverAge { get; set; }

    public int Accidents { get; set; }

    public int Violations { get; set; }

    public string CoverageLevel { get; set; } = "Full";
}