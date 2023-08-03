namespace FSFlightBuilder.Data.Models;

public partial class Parking
{
    public long Id { get; set; }

    public string? AirportId { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public long? Number { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public long? Heading { get; set; }

    public long? GateType { get; set; }

    public long? FSType { get; set; }

    public virtual Airport? Airport { get; set; }
}
