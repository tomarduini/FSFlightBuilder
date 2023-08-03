using FSFlightBuilder.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSFlightBuilder.Data
{
    public interface ISqlDatabase
    {
        Task<IEnumerable<Aircraft>> GetAircraft();
        IEnumerable<Comm> GetComms();
        IEnumerable<Parking> GetParkings();
        IEnumerable<Airport> GetAirports();
        Airport GetAirport(string apt);
        Navaid GetNavaidById(string id);
        Waypoint GetWaypointById(string id);
        void SaveAircraft(List<Aircraft> aircraft);
    }
}
