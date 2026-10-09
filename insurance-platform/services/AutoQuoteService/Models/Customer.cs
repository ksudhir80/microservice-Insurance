namespace AutoQuoteService.Models;

public class VehicleInfo
{
    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Vin { get; set; } = string.Empty;
}