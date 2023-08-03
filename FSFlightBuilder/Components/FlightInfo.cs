using System;
using System.Collections.Generic;

namespace FSFlightBuilder.Components
{
    internal class FlightInfo
    {
        public string FlightFile { get; set; }
        public string FlightPlanFile { get; set; }
        public string BriefingFile { get; set; }
        public string XmlFile { get; set; }
        public List<string> Route { get; set; }
        public FSFlightBuilder.Data.Models.Aircraft Aircraft { get; set; }
        public string Parking { get; set; }
        public string CruiseAltitude { get; set; }
        public string CruiseSpeed { get; set; }
        public DateTime FlightTime { get; set; }
        public string RouteType { get; set; }
        public string FpType { get; set; }
        public int Runway { get; set; }
        public string FlightType { get; set; }
        public string WeatherType { get; set; }
        public string WeatherTheme { get; set; }
    }
}
