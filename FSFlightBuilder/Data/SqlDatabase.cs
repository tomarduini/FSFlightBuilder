using FSFlightBuilder.Components;
using FSFlightBuilder.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FSFlightBuilder.Data
{

    //class SqlTransaction;
    //class SqlQuery;
    //class SqlRecord;

    /*
     * Wrapper around QSqlDatabase that adds exceptions to avoid plenty of
     * boilerplate coding. In case of error or invalid connections SqlException
     * is thrown. The driver has to support transactions.
     * This class also add a normal commit/rollback mechanism which always keeps an
     * transaction open.
     */
    public class SqlDatabase : ISqlDatabase
    {
        private readonly FSFBDbConn ctx;
        public SqlDatabase(FSFBDbConn ctx)
        {
            this.ctx = ctx;
        }

        public void Close()
        {
            ctx.Dispose();
        }

        public async Task<IEnumerable<Aircraft>> GetAircraft()
        {
            return await ctx.Aircraft.ToListAsync();
        }

        public IEnumerable<Airport> GetAirports()
        {
            //return ctx.Airports.Include(r => r.Runways).ToList();
            //return await ctx.Airports.Include("Runways").Include("Parkings").Include("Comms").ToListAsync();
            return ctx.Airports.Include("Runways").Include("Comms").Include("Parkings");
        }

        public IEnumerable<Comm> GetComms()
        {
            //return ctx.Airports.Include(r => r.Runways).ToList();
            //return await ctx.Airports.Include("Runways").Include("Parkings").Include("Comms").ToListAsync();
            return ctx.Comms;
        }

        public IEnumerable<Parking> GetParkings()
        {
            //return ctx.Airports.Include(r => r.Runways).ToList();
            //return await ctx.Airports.Include("Runways").Include("Parkings").Include("Comms").ToListAsync();
            return ctx.Parkings;
        }

        public Airport GetAirport(string icao)
        {
            return ctx.Airports.Include("Runways").Include("Parkings").Include("Comms").FirstOrDefault(a => a.AirportId == icao);
        }

        public Navaid GetNavaidById(string id)
        {
            return ctx.Navaids.FirstOrDefault(n => n.NavId == id);
        }

        public Waypoint GetWaypointById(string id)
        {
            return ctx.Waypoints.Include("Routes").FirstOrDefault(n => n.NavId == id);
        }

        public void SaveAircraft(List<Aircraft> aircraft)
        {
            foreach (var acft in ctx.Aircraft)
            {
                var a = aircraft.FirstOrDefault(ac => (int)ac.Id == (int)acft.Id);
                if (AWDConvert.ToDecimal(a.DescentRate) > 0 || AWDConvert.ToDecimal(a.DescentSpeed) > 0)
                {
                    acft.DescentRate = a.DescentRate;
                    acft.DescentSpeed = a.DescentSpeed;
                }
            }
            ctx.SaveChanges();
        }
    }

}
