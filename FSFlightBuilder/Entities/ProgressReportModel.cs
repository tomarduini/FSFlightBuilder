using System.Collections.Generic;

namespace FSFlightBuilder.Entities
{
    internal class ProgressReportModel
    {
        public int Progress { get; set; } = 0;
        public int PctProgress { get; set; } = 0;
        public int MaxFiles { get; set; } = 0;
        public int AirportCount { get; set; } = 0;
        public int RunwayCount { get; set; } = 0;
        public int CommCount { get; set; } = 0;
        public int ParkingCount { get; set; } = 0;
        public int NavaidCount { get; set; } = 0;
        public int WaypointsCount { get; set; } = 0;
        public int RoutesCount { get; set; } = 0;
        public List<string> Aircraft { get; set; } = new List<string>();
        public string Caption { get; set; } = null;
        public string folder { get; set; }
        public string file { get; set; }
        public bool Saving { get; set; } = false;
        public bool Finished { get; set; } = false;
    }
}
