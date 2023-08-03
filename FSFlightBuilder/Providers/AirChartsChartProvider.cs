using FSFlightBuilder.Components;
using FSFlightBuilder.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FSFlightBuilder.Providers
{
    public class AirChartsChartProvider : IChartProvider
    {

        public IEnumerable<Chart> GetCharts(string departure, string destination)
        {

            var charts = new List<Chart>();
            for (var idx = 0; idx < 2; idx++)
            {
                var apt = idx == 0 ? departure : destination;
                string chartResult = string.Empty;
                try
                {

                    using (var client = new HttpClient())
                    {
                        var url = "https://api.aircharts.org/v2/Airport/" + apt;
                        var response = client.GetAsync(url).Result;

                        if (response.IsSuccessStatusCode)
                        {

                            // by calling .Result you are performing a synchronous call
                            var responseContent = response.Content;

                            // by calling .Result you are synchronously reading the result
                            chartResult = responseContent.ReadAsStringAsync().Result;

                            var xmlDoc = JsonConvert.DeserializeXNode(chartResult, "Root");
                            var airports =
                                from airport in
                                xmlDoc.Elements("Root")
                                select airport;

                            foreach (var airport in airports.Elements())
                            {
                                var name = airport.Element("info").Element("name");
                                if (name != null)
                                {
                                    foreach (var item in airport.Elements("charts"))
                                    {
                                        var i = -1;
                                        foreach (var elem in item.Elements())
                                        {
                                            i++;
                                            var type = elem.Name.ToString();
                                            ChartTypes chartType;
                                            switch (type)
                                            {
                                                case "General":
                                                    chartType = ChartTypes.General;
                                                    break;
                                                case "Approach":
                                                    chartType = ChartTypes.Approach;
                                                    break;
                                                case "SID":
                                                    chartType = ChartTypes.Departure;
                                                    break;
                                                case "STAR":
                                                    chartType = ChartTypes.Arrival;
                                                    break;
                                                default:
                                                    chartType = ChartTypes.Miscellaneous;
                                                    break;
                                            }
                                            charts.Add(new Chart
                                            {
                                                ChartId = elem.Element("id").Value,
                                                ChartName = elem.Element("chartname").Value,
                                                ChartType = chartType,
                                                Runway = string.Empty,
                                                ICAO = apt,
                                                PDFUrl = elem.Element("url").Value.Replace("http://", "https://"),
                                                AirportName = name.Value
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to connect to the chart service.  Please check your internet connection and try again.");
                    charts.Add(new Chart
                    {
                        ICAO = apt,
                        AirportName = "Error getting charts",
                        ChartType = ChartTypes.None
                    });
                }
            }

            return charts;
        }
    }
}
