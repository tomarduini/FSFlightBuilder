using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("parking")]
public partial class DbParking
{
    [Key]
    public int parking_id { get; set; }

    public int airport_id { get; set; }

    public string? type { get; set; }

    public string? name { get; set; }

    public long? number { get; set; }

    public string? suffix { get; set; }

    public double? laty { get; set; }

    public double? lonx { get; set; }

    public long? heading { get; set; }

    //public long? GateType { get; set; }

    //public long? FSType { get; set; }

    public virtual DbAirport? Airport { get; set; }
}
