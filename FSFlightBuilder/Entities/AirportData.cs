using FSFlightBuilder.Data.Models;
using System.Collections.Generic;

namespace FSFlightBuilder.Entities
{
    internal class AirportData
    {
        public string ICAO { get; set; }
        public string Name { get; set; }
        public int Elevation { get; set; }
        public double Distance { get; set; }
        public int MaxRunLength { get; set; }
        public int MaxRunWidth { get; set; }
        public bool Towered { get; set; }
        public bool HasHardRunway { get; set; }
        public bool HasAvGas { get; set; }
        public bool HasJetFuel { get; set; }
        public bool HasILS { get; set; }
        public IEnumerable<Parking> Parkings { get; set; }
        public IEnumerable<Comm> Comms { get; set; }
    }
}
