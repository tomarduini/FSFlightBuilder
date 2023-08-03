using FSFlightBuilder.Data.Models;
using FSFlightBuilder.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSFlightBuilder.Components
{
    public static class DataHelper
    {
        public static List<Airport> airports;
        public static List<Airport> xmlAirports;
        public static List<Aircraft> xmlAircraft;
        public static List<Navaid> xmlNavaids;
        public static List<Waypoint> xmlWaypoints;
        private static List<Comm> Comms;
        private static List<Parking> Parkings;

        public static List<Airport> AllAirports
        {
            get
            {
                if (Common.DataUpdateInProgress)
                {
                    return xmlAirports;
                }
                else
                {
                    return airports;
                }
            }
        }

        public static Airport GetAirport(string icao)
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                var runs = ctx.Runways.Where(r => r.AirportId == icao && r.FSType == (int)Common.FlightSim);
                var parks = ctx.Parkings.Where(p => p.AirportId == icao && p.FSType == (int)Common.FlightSim);
                var comms = ctx.Comms.Where(c => c.AirportId == icao && c.FSType == (int)Common.FlightSim);

                var apt = ctx.Airports.Include(r => r.Runways).Include(p => p.Parkings).Include(c => c.Comms).FirstOrDefault(a => a.AirportId == icao && a.FSType == (int)Common.FlightSim);
                if (apt != null)
                {
                    apt.Runways = runs.ToList();
                    apt.Parkings = parks.ToList();
                    apt.Comms = comms.ToList();
                }
                return apt;
            }
        }

        public static async Task<List<Aircraft>> GetAircraft()
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                return await ctx.Aircraft.Where(a => a.FSType == (int)Common.FlightSim).OrderBy(a => a.Name).ToListAsync();
            }
        }

        public static List<Airport> GetAirports()
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                return ctx.Airports.Include("Runways").Where(a => a.FSType == (int)Common.FlightSim).ToList();
            }
        }

        public static List<Comm> GetComms()
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                return ctx.Comms.Where(a => a.FSType == (int)Common.FlightSim).ToList();
            }
        }

        public static List<Data.Models.Parking> GetParkings()
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                var parkings = ctx.Parkings
                    .Where(p => p.FSType == (int)Common.FlightSim).ToList();
                foreach (var parking in parkings)
                {
                    parking.Name = $"{ParkingUtil.parkingNameToStr(parking.Name)} ({ParkingUtil.parkingTypeToStr(parking.Type)})";
                }
                return parkings;
            }
        }

        public static async Task<List<Navaid>> GetNavaids()
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                return await ctx.Navaids
                    .Where(p => p.FSType == (int)Common.FlightSim).ToListAsync();
            }
        }

        public static async Task<List<Waypoint>> GetWaypoints()
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                return await ctx.Waypoints.Include(w => w.Routes)
                    .Where(p => p.FSType == (int)Common.FlightSim).ToListAsync();
            }
        }

        internal static IEnumerable<Waypoint> GetWaypointsById(string id)
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                return ctx.Waypoints.Include(w => w.Routes)
                    .Where(p => p.NavId == id && p.FSType == (int)Common.FlightSim).ToList();
            }
        }

        public static Navaid GetNavaidById(string id)
        {
            if (Common.DataUpdateInProgress)
            {
                if (xmlNavaids != null)
                {
                    return xmlNavaids.FirstOrDefault(n => n.NavId == id);
                }
                return null;
            }
            else
            {
                using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
                {
                    return ctx.Navaids.FirstOrDefault(n => n.NavId == id && n.FSType == (int)Common.FlightSim);
                }
            }
        }

        public static Waypoint GetWaypointById(string id)
        {
            if (Common.DataUpdateInProgress)
            {
                if (xmlWaypoints != null)
                {
                    return xmlWaypoints.FirstOrDefault(n => n.NavId == id);
                }
                return null;
            }
            else
            {
                using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
                {
                    return ctx.Waypoints.Include("Routes").FirstOrDefault(n => n.NavId == id && n.FSType == (int)Common.FlightSim);
                }
            }
        }

        public static void SaveAircraft(List<Aircraft> aircraft)
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                foreach (var acft in aircraft)
                {
                    //Update all aircraft
                    var dbAircraft = ctx.Aircraft.Where(a => a.UIType.ToLower() == acft.UIType.ToLower()).ToList();
                    dbAircraft.ForEach(s => s.Airspeed = acft.Airspeed);
                    dbAircraft.ForEach(s => s.ClimbSpeed = acft.ClimbSpeed);
                    dbAircraft.ForEach(s => s.ClimbRate = acft.ClimbRate);
                    dbAircraft.ForEach(s => s.DescentSpeed = acft.DescentSpeed);
                    dbAircraft.ForEach(s => s.DescentRate = acft.DescentRate);
                    ctx.UpdateRange(dbAircraft);
                }
                ctx.SaveChanges();



                //foreach (var acft in ctx.Aircraft.Where(a => a.FSType == (int)Common.FlightSim))
                //{
                //    var a = aircraft.FirstOrDefault(ac => ac.Name == acft.Name && ac.FSType == (int)Common.FlightSim);
                //    if (Convert.ToDecimal(a.DescentRate) > 0 || Convert.ToDecimal(a.DescentSpeed) > 0)
                //    {
                //        acft.DescentRate = a.DescentRate;
                //        acft.DescentSpeed = a.DescentSpeed;
                //    }
                //}
                //ctx.SaveChanges();
            }
        }

        internal static Aircraft GetAircraftByName(string name)
        {
            if (Common.DataUpdateInProgress)
            {
                if (xmlAircraft != null)
                {
                    return xmlAircraft.FirstOrDefault(a => a.Name == name);
                }
                return null;
            }
            else
            {
                using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
                {
                    return ctx.Aircraft.FirstOrDefault(a => a.Name == name && a.FSType == (int)Common.FlightSim);
                }
            }
        }

        internal static async Task<bool> GetAllAirports()
        {
            if (Common.DataUpdateInProgress)
            {
                if (xmlAirports != null)
                {
                    airports = xmlAirports;
                }
            }
            else
            {
                using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
                {
                    airports = await ctx.Airports.Include(r => r.Runways)
                    .Where(ap => ap.FSType == (int)Common.FlightSim).ToListAsync();
                }
            }
            return true;
        }

        internal static void AddDatabaseColumns()
        {
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                RemoveForeignKey(ctx, "Runways");
                RemoveForeignKey(ctx, "Routes");

                try
                {
                    RemoveForeignKey(ctx, "Comm");
                    //See if the Comm table exists
                    var recs = ctx.Database
                        .SqlQuery<int>($"SELECT COUNT(name) FROM sqlite_master WHERE name ='Comm' and type='table'")
                        .ToList();
                    if (recs.Count > 0 && recs[0] > 0)
                    {
                        //This is the old database
                        var rawSql = $"ALTER TABLE Comm RENAME TO Comms";
                        var result = ctx.Database.ExecuteSqlRaw(rawSql);
                    }
                }
                catch { }
                try
                {
                    RemoveForeignKey(ctx, "Parking");
                    //See if the Parking table exists
                    var recs = ctx.Database
                        .SqlQuery<int>($"SELECT COUNT(name) FROM sqlite_master WHERE name ='Parking' and type='table'")
                        .ToList();
                    if (recs.Count > 0 && recs[0] > 0)
                    {
                        //This is the old database
                        var rawSql = $"ALTER TABLE Parking RENAME TO Parkings";
                        var result = ctx.Database.ExecuteSqlRaw(rawSql);
                    }
                }
                catch { }
                try
                {
                    //Check if the HasTower column exists. If not, it's the old database.
                    var recs = ctx.Database
                        .SqlQuery<int>($"SELECT COUNT(*) AS column_exists FROM pragma_table_info('Airports') WHERE name='HasTower'")
                        .ToList();
                    if (recs.Count == 0 || recs[0] == 0)
                    {
                        //The column doesn't exist, which means none of the new columns will exist
                        var rawSql = $"ALTER TABLE Airports ADD HasTower INTEGER";
                        var result = ctx.Database.ExecuteSqlRaw(rawSql);
                        rawSql = $"ALTER TABLE Airports ADD HasAvGas INTEGER";
                        result = ctx.Database.ExecuteSqlRaw(rawSql);
                        rawSql = $"ALTER TABLE Airports ADD HasJetFuel INTEGER";
                        result = ctx.Database.ExecuteSqlRaw(rawSql);
                        rawSql = $"ALTER TABLE Airports ADD LongestRwyLength INTEGER";
                        result = ctx.Database.ExecuteSqlRaw(rawSql);
                        rawSql = $"ALTER TABLE Airports ADD LongestRwyWidth INTEGER";
                        result = ctx.Database.ExecuteSqlRaw(rawSql);
                    }
                }
                catch { }
            }
        }

        private static void RemoveForeignKey(FSFBDbConn ctx, string tableName)
        {
            var rawSql = string.Empty;
            try
            {
                switch (tableName)
                {
                    case "Comm":
                        rawSql = $@"CREATE TABLE {tableName}_new(
                        Id INTEGER PRIMARY KEY,
                        AirportId TEXT,
                        Type INTEGER,
                        Frequency TEXT,
                        Name TEXT,
                        FSType INT NOT NULL)";
                        break;
                    case "Parking":
                        rawSql = $@"CREATE TABLE {tableName}_new(
                        Id INTEGER PRIMARY KEY,
                        AirportId TEXT,
                        Type TEXT,
                        Name TEXT,
                        Number INTEGER,
                        Latitude DOUBLE,
                        Longitude DOUBLE,
                        Heading INTEGER,
                        GateType INTEGER,
                        FSType INT NOT NULL)";
                        break;
                    case "Runways":
                        rawSql = $@"CREATE TABLE {tableName}_new(
                        Id INTEGER PRIMARY KEY,
                        AirportId TEXT,
                        Number TEXT,
                        Length INTEGER,
                        Width INTEGER,
                        Latitude DOUBLE,
                        Longitude DOUBLE,
                        Heading INTEGER,
                        Surface TEXT,
                        IlsFrequency TEXT,
                        IlsHeading DECIMAL,
                        ApproachLights TEXT,
                        Glideslope TEXT,
                        PatternTakeoff TEXT,
                        PatternLanding TEXT,
                        PatternAltitude TEXT,
                        FSType INT NOT NULL)";
                        break;
                    case "Routes":
                        rawSql = $@"CREATE TABLE {tableName}_new(
                        Id INTEGER PRIMARY KEY,
                        WaypointId INTEGER,
                        WaypointNavId TEXT,
                        Name TEXT,
                        Type TEXT,
                        Next TEXT,
                        Previous TEXT,
                        FSType INT NOT NULL)";
                        break;
                }
                ctx.Database.ExecuteSqlRaw(rawSql);

                rawSql = $"INSERT INTO {tableName}_new SELECT * FROM {tableName}";
                ctx.Database.ExecuteSqlRaw(rawSql);

                rawSql = $"DROP TABLE {tableName}";
                ctx.Database.ExecuteSqlRaw(rawSql);

                rawSql = $"ALTER TABLE {tableName}_new RENAME TO {tableName}";
                ctx.Database.ExecuteSqlRaw(rawSql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        internal static bool CheckDatabase()
        {
            //Check to see if Comms exists 
            //SELECT EXISTS(SELECT name FROM sqlite_schema WHERE type = 'table' AND name = 'Comms');
            using (var ctx = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                try
                {
                    var rawSql = $"SELECT name FROM sqlite_master WHERE type='table' AND name='Comms'";
                    DbSet<SqlMaster> sqlMaster = ctx.Set<SqlMaster>();
                    var master = sqlMaster.FromSqlRaw(rawSql);
                    return master.Any();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

    }
}
