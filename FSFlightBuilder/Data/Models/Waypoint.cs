using System.Collections.Generic;

namespace FSFlightBuilder.Data.Models;

public partial class Waypoint
{
    public long Id { get; set; }

    //public long WaypointId { get; set; }

    public string NavId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Region { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double MagVar { get; set; }

    public long FSType { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
