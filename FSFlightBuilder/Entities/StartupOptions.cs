using System;
using System.Collections.Generic;

namespace FSFlightBuilder.Entities
{
    internal class StartupOptions
    {
        internal DateTime FlightDate = DateTime.Now;
        internal string LastFlightTime = string.Empty;
        internal List<String> WeatherTypes = new List<String>();
        internal bool IncludeTOD = false;
        //        internal bool UseSystem = false;
    }
}
