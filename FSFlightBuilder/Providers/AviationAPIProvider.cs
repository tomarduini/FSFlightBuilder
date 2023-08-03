using FSFlightBuilder.Components;
using FSFlightBuilder.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FSFlightBuilder.Providers
{
    public class AviationAPIProvider : IChartProvider
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
                    if (Char.IsNumber(apt[0]))
                    {
                        apt = "K" + apt;
                    }


                    using (var client = new HttpClient())
                    {
                        var url = "https://api.aviationapi.com/v1/charts?apt=" + apt + "&group=1";
                        var response = client.GetAsync(url).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            // by calling .Result you are performing a synchronous call
                            var responseContent = response.Content;

                            // by calling .Result you are synchronously reading the result
                            chartResult = responseContent.ReadAsStringAsync().Result;
                            var xmlDoc = JsonConvert.DeserializeXNode(chartResult, apt);
                            var airports =
                                from airport in
                                xmlDoc.Elements(apt)
                                select airport;

                            foreach (var airport in airports.Elements())
                            {
                                foreach (var item in airport.Elements())
                                {
                                    var type = item.Name.ToString();
                                    ChartTypes chartType;
                                    string runway = string.Empty;
                                    switch (type)
                                    {
                                        case "General":
                                            chartType = ChartTypes.General;
                                            break;
                                        case "CAPP":
                                            chartType = ChartTypes.Approach;
                                            if (item.Element("chart_name") != null && !string.IsNullOrEmpty(item.Element("chart_name").Value))
                                            {
                                                try
                                                {
                                                    var cht = item.Element("chart_name").Value;
                                                    var index = cht.ToUpper().IndexOf("RWY");
                                                    if (index > -1)
                                                    {
                                                        cht = cht.Substring(index);
                                                        var i = cht.IndexOf(" ", 4);
                                                        cht = cht.Substring(4, 2);
                                                        if (!string.IsNullOrEmpty(cht))
                                                        {
                                                            runway = cht;
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    var str = ex.Message;
                                                }
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
                                    var addChart = ((apt == departure && chartType != ChartTypes.Approach && chartType != ChartTypes.Arrival) || (apt == destination && chartType != ChartTypes.Departure));
                                    if (addChart)
                                    {
                                        charts.Add(new Chart
                                        {
                                            ChartId = item.Element("pdf_name").Value,
                                            ChartName = item.Element("chart_name").Value,
                                            ChartType = chartType,
                                            Runway = runway,
                                            ICAO = apt,
                                            PDFUrl = item.Element("pdf_path").Value.Replace("http://", "https://"),
                                            AirportName = item.Element("airport_name").Value
                                        });
                                    }
                                }
                            }
                        }
                        //Get the AFD
                        url = "https://api.aviationapi.com/v1/charts/afd?apt=" + apt;
                        response = client.GetAsync(url).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            // by calling .Result you are performing a synchronous call
                            var responseContent = response.Content;

                            // by calling .Result you are synchronously reading the result
                            chartResult = responseContent.ReadAsStringAsync().Result;

                            var xmlDoc = JsonConvert.DeserializeXNode("{\"" + apt + "\":" + chartResult + "}", apt);

                            var airports =
                                                from airport in
                                                xmlDoc.Elements(apt)
                                                select airport;

                            foreach (var airport in airports.Elements())
                            {
                                charts.Add(new Chart
                                {
                                    ChartId = airport.Element("pdf_name").Value,
                                    ChartName = "Airport Facility Directory",
                                    ChartType = ChartTypes.AFD,
                                    Runway = string.Empty,
                                    ICAO = apt,
                                    PDFUrl = airport.Element("pdf_path").Value.Replace("http://", "https://"),
                                    AirportName = airport.Element("airport_name").Value
                                });
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
