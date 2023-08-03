using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSFlightBuilder.Data.Models;

[Table("com")]
public partial class DbComm
{
    [Key]
    public int com_id { get; set; }

    public int airport_id { get; set; }

    public string? type { get; set; }

    public int frequency { get; set; }

    public string? name { get; set; }

    public virtual DbAirport? Airport { get; set; }
}
