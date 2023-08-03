namespace FSFlightBuilder.Components
{
    internal class RoutePoint
    {
        public string Id { get; set; }
        public string Region { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Type { get; set; }
        public string Frequency { get; set; }
        public double MagVar { get; set; }
        public double Elevation { get; set; }
    }
}
