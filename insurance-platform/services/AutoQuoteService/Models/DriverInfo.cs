namespace AutoQuoteService.Models;

public class DriverInfo
{
    public DateOnly DateOfBirth { get; set; }

    public string LicenseState { get; set; } = string.Empty;

    public int Accidents { get; set; }

    public int Violations { get; set; }
}