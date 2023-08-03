using System.Collections.Generic;

namespace FSFlightBuilder.Data.Models;

public partial class Airport
{
    public string AirportId { get; set; } = null!;

    public string? IcaoName { get; set; }

    public long? Elevation { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double MagVar { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public long? WindDirection { get; set; }

    public long? WindSpeed { get; set; }

    public int? HasTower { get; set; }
    public int? HasAvGas { get; set; }
    public int? HasJetFuel { get; set; }
    public int? LongestRwyLength { get; set; }
    public int? LongestRwyWidth { get; set; }

    public long FSType { get; set; }

    public virtual ICollection<Comm> Comms { get; set; } = new List<Comm>();

    public virtual ICollection<Parking> Parkings { get; set; } = new List<Parking>();

    public virtual ICollection<Runway> Runways { get; set; } = new List<Runway>();
}
