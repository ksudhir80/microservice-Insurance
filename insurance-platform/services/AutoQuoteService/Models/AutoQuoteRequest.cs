namespace AutoQuoteService.Models;

public class AutoQuoteRequest
{
    public int CustomerId { get; set; }

    public VehicleInfo Vehicle { get; set; } = new();

    public DriverInfo Driver { get; set; } = new();

    public CoverageInfo Coverage { get; set; } = new();
}