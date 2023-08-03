namespace FSFlightBuilder.Data.Models;

public partial class Comm
{
    public long Id { get; set; }

    public string? AirportId { get; set; }

    public long? Type { get; set; }

    public string? Frequency { get; set; }

    public string? Name { get; set; }

    public long? FSType { get; set; }

    public virtual Airport? Airport { get; set; }
}
