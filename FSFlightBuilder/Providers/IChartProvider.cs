using FSFlightBuilder.Components;
using System.Collections.Generic;

namespace FSFlightBuilder.Providers
{
    public interface IChartProvider
    {
        IEnumerable<Chart> GetCharts(string departure, string destination);
    }
}
