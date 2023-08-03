using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("airport")]
public class DbAirport
{
    [Key]
    public int airport_id { get; set; }

    public string ident { get; set; } = null!;

    public string? name { get; set; }

    public long? altitude { get; set; }

    public double laty { get; set; }

    public double lonx { get; set; }

    public double mag_var { get; set; }

    public string? country { get; set; }

    public string? state { get; set; }

    public string? city { get; set; }

    public virtual ICollection<DbComm> Comms { get; set; } = new List<DbComm>();

    public virtual ICollection<DbParking> Parkings { get; set; } = new List<DbParking>();

    public virtual ICollection<DbRunway> Runways { get; set; } = new List<DbRunway>();
    //public virtual ICollection<DbVor> VORs { get; set; } = new List<DbVor>();
    //public virtual ICollection<DbNdb> NDBs { get; set; } = new List<DbNdb>();
}
