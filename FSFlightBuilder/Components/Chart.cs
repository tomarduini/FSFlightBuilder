using FSFlightBuilder.Enums;

namespace FSFlightBuilder.Components
{
    public class Chart
    {
        public string ChartId { get; set; }
        public ChartTypes ChartType { get; set; } //chart_code
        public string ChartName { get; set; } //chart_name
        public string Runway { get; set; } //Check faanfd18 element for two characters after the first character
        public string ICAO { get; set; } //airport_name icao_ident attribute
        public string PDFUrl { get; set; }
        public string AirportName { get; set; }
    }
}
