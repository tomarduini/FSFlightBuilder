namespace FSFlightBuilder.Data.Models;

public partial class Runway
{
    public long Id { get; set; }

    public string? AirportId { get; set; }

    public string? Number { get; set; }

    public long? Length { get; set; }

    public long? Width { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public decimal? Heading { get; set; }

    public string? Surface { get; set; }

    public string? IlsFrequency { get; set; }

    public decimal? IlsHeading { get; set; }

    public string? ApproachLights { get; set; }

    public string? Glideslope { get; set; }

    public string? PatternTakeoff { get; set; }

    public string? PatternLanding { get; set; }

    public string? PatternAltitude { get; set; }

    public long? FSType { get; set; }

    public virtual Airport? Airport { get; set; }
}
