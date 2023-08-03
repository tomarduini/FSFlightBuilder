namespace FSFlightBuilder.Data.Models;

public partial class Route
{
    public long Id { get; set; }

    public long WaypointId { get; set; }

    public string WaypointNavId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    //public long NextId { get; set; }
    public string? Next { get; set; }

    //public long PreviousId { get; set; }
    public string? Previous { get; set; }

    public long FSType { get; set; }

    public virtual Waypoint? Waypoint { get; set; }
}
