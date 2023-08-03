using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("runway_end")]
public class DbRunwayEnd
{
    [Key]
    public int runway_end_id { get; set; }

    public string name { get; set; }

    public int? altitude { get; set; }

    public double heading { get; set; }

    public string? app_light_system_type { get; set; }

    public double laty { get; set; }

    public double lonx { get; set; }

    public string? left_vasi_type { get; set; }

    public double? left_vasi_pitch { get; set; }

    public string? right_vasi_type { get; set; }

    public double? right_vasi_pitch { get; set; }

    public string? ils_ident { get; set; }

    //public virtual DbRunway? Runway { get; set; }
    //public virtual DbILS? ILS { get; set; }
}
