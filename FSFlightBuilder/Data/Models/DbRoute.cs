namespace FSFlightBuilder.Data.Models;

public partial class DbRoute
{
    public int route_id { get; set; }

    public int waypoint_id { get; set; }

    //public string WaypointNavId { get; set; } = null!;

    //public string Name { get; set; } = null!;

    //public string? Type { get; set; }

    //public string? Next { get; set; }

    //public string? Previous { get; set; }

    //public long FSType { get; set; }

    public virtual DbWaypoint? Waypoint { get; set; }
}
