using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("vor")]
public class DbVor
{
    [Key]
    public int vor_id { get; set; }

    public string ident { get; set; } = null!;

    public string? name { get; set; }

    public int frequency { get; set; } = 0;

    public double laty { get; set; }

    public double lonx { get; set; }

    public long altitude { get; set; }

    public string? type { get; set; }

    public string? region { get; set; }

    public double? mag_var { get; set; }

    //public virtual DbAirport? Airport { get; set; }
}

