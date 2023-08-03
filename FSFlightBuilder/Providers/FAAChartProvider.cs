//using atools.fs.utils;
using FSFlightBuilder.Components;
using FSFlightBuilder.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// using System.Net.Http;
using System.Xml.Linq;

namespace FSFlightBuilder.Providers
{
    public class FAAChartProvider : IChartProvider
    {
        private string _dataPath;

        public FAAChartProvider(string path)
        {
            _dataPath = path;
        }

        public IEnumerable<Chart> GetCharts(string departure, string destination)
        {
            var charts = new List<Chart>();
            var helper = new ResourceHelpers();
            // http://aeronav.faa.gov/d-tpp/1902/xml_data/d-tpp_Metafile.xml

            var airac = Common.CheckAirac();

            // Don't make this a resource
            if (File.Exists(_dataPath + @"\d-tpp_Metafile.xml"))
            {
                var xmlDoc = XDocument.Load(_dataPath + @"\d-tpp_Metafile.xml");
                var allAirports =
                    from item in
                    xmlDoc.Elements("digital_tpp")
                        .Elements("state_code")
                        .Elements("city_name")
                        .Elements("airport_name")
                    select item;

                var airports = allAirports.Where(a => a.Attribute("icao_ident").Value == departure || a.Attribute("icao_ident").Value == destination);

                foreach (var airport in airports)
                {
                    var name = airport.Attribute("ID");
                    var icao = airport.Attribute("icao_ident");
                    if (name != null && icao != null)
                    {
                        foreach (var item in airport.Elements("record"))
                        {
                            ChartTypes chartType;
                            string runway = string.Empty;
                            switch (item.Element("chart_code").Value)
                            {
                                case "APD":
                                    chartType = ChartTypes.General;
                                    break;
                                case "IAP":
                                    chartType = ChartTypes.Approach;
                                    if (item.Element("faanfd18") != null && !string.IsNullOrEmpty(item.Element("faanfd18").Value))
                                    {
                                        runway = item.Element("faanfd18").Value.Substring(1, 2);
                                    }
                                    break;
                                case "DP":
                                    chartType = ChartTypes.Departure;
                                    break;
                                case "STAR":
                                    chartType = ChartTypes.Arrival;
                                    break;
                                default:
                                    chartType = ChartTypes.Miscellaneous;
                                    break;
                            }
                            var addChart = ((icao.Value == departure && chartType != ChartTypes.Approach && chartType != ChartTypes.Arrival) || (icao.Value == destination && chartType != ChartTypes.Departure));
                            if (addChart)
                            {
                                charts.Add(new Chart
                                {
                                    ChartId = string.IsNullOrEmpty(item.Element("procuid").Value) ? item.Element("chartseq").Value : item.Element("procuid").Value,
                                    ChartName = item.Element("chart_name").Value,
                                    ChartType = chartType,
                                    Runway = runway,
                                    ICAO = icao.Value,
                                    PDFUrl = $"http://aeronav.faa.gov/d-tpp/{airac}/" + item.Element("pdf_name").Value,
                                    AirportName = name.Value
                                });
                            }
                        }
                    }
                }
            }
            else
            {
                charts.Add(new Chart
                {
                    ICAO = departure,
                    AirportName = "Error getting charts",
                    ChartType = ChartTypes.None
                });
            }

            return charts;
        }
    }
}
