using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("runway")]
public class DbRunway
{
    [Key]
    public int runway_id { get; set; }

    public int airport_id { get; set; }

    //[ForeignKey("RunwayEnds")]
    public int primary_end_id { get; set; }

    //[ForeignKey("RunwayEnds")]
    public int secondary_end_id { get; set; }

    public string? surface { get; set; }

    public double length { get; set; }

    public double width { get; set; }

    public int pattern_altitude { get; set; }

    public virtual DbAirport? Airport { get; set; }

    //public virtual ICollection<DbRunwayEnd> RunwayEnds { get; set; } = new List<DbRunwayEnd>();
    //public virtual DbRunwayEnd? RunwayEnd { get; set; }
}
