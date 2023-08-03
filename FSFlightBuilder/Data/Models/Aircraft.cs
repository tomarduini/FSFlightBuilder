namespace FSFlightBuilder.Data.Models;

public partial class Aircraft
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Texture { get; set; } = null!;

    public string UIType { get; set; } = null!;

    public decimal? Airspeed { get; set; } = 0;

    public decimal? ClimbSpeed { get; set; } = 0;

    public decimal? ClimbRate { get; set; } = 0;

    public decimal? DescentSpeed { get; set; } = 0;

    public decimal? DescentRate { get; set; } = 0;

    public string? Unit { get; set; }

    public long FSType { get; set; }
}
