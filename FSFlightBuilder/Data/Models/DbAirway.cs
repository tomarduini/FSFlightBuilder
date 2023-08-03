using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("airway")]
public partial class DbAirway
{
    [Key]
    public long airway_id { get; set; }

    public string airway_name { get; set; } = null!;

    public string airway_type { get; set; } = null!;

    public long to_waypoint_id { get; set; }

    public long from_waypoint_id { get; set; }
}
