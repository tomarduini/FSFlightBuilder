using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("waypoint")]
public class DbWaypoint
{
    [Key]
    public int waypoint_id { get; set; }

    public string ident { get; set; } = null!;

    public string type { get; set; } = null!;

    public string? region { get; set; }

    public double laty { get; set; }

    public double lonx { get; set; }

    public double mag_var { get; set; }
}
