using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("ils")]
public class DbILS
{
    [Key]
    public int ils_id { get; set; }

    [ForeignKey("RunwayEnds")]
    public int loc_runway_end_id { get; set; }

    public string? name { get; set; }

    public int? frequency { get; set; }

    public double? loc_heading { get; set; }

    //public virtual ICollection<DbRunwayEnd> RunwayEnds { get; set; } = new List<DbRunwayEnd>();
}
