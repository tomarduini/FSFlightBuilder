namespace FSFlightBuilder.Data.Models;

public partial class Navaid
{
    public long Id { get; set; }

    public string NavId { get; set; } = null!;

    public string? Name { get; set; }

    public string Frequency { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public long Elevation { get; set; }

    public string? Type { get; set; }

    public string? Region { get; set; }

    public double? MagVar { get; set; }

    public long FSType { get; set; }
}
