using FSFB.Data.Models;
using FSFlightBuilder.Components;
using FSFlightBuilder.Data.Models;
using FSFlightBuilder.Entities;
using FSFlightBuilder.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FSFlightBuilder
{
    public partial class BGLImporter : Form
    {
        private List<Data.Models.Airport> airports = new List<Data.Models.Airport>();
        private List<Navaid> navaids = new List<Navaid>();
        private List<Data.Models.Waypoint> waypoints = new List<Data.Models.Waypoint>();
        private List<Route> routes = new List<Route>();
        private List<Data.Models.Runway> runways = new List<Data.Models.Runway>();
        private List<Data.Models.Parking> parkings = new List<Data.Models.Parking>();
        private List<Comm> comms = new List<Comm>();
        private List<Aircraft> aircraft = new List<Aircraft>();
        private List<string> aircraftFiles = new List<string>();

        private List<Airway> dbairways = new List<Airway>();
        private List<FSFB.Data.Models.Waypoint> dbwaypoints = new List<FSFB.Data.Models.Waypoint>();
        private ProgressReportModel model = new ProgressReportModel();

        private double factor = 0.0;
        private int oldPercentage = 0;

        public event EventHandler<EventArgs> Canceled;

        AWDControls.WaitWndFun waitForm = new AWDControls.WaitWndFun();

        public BGLImporter()
        {
            InitializeComponent();

            try
            {
                Common.logger.Info($"Starting the Importer functionality");
                UseWaitCursor = true;
                btnCancel.UseWaitCursor = false;
                ClearLists();

                if (Common.FlightSim == FlightSimType.Unknown)
                {
                    MessageBox.Show(
                        @"FS Flight Builder could not determin the Flight Simulator.  Please select the Flight Simulator from the Options menu.");
                    DialogResult = DialogResult.Cancel;
                    Close();
                }

                //Make a backup of the current database and call it fsflightbuildertmp.db
                if (File.Exists($"{Common.DataPath}\\fsflightbuilder.db"))
                {
                    File.Copy($"{Common.DataPath}\\fsflightbuilder.db", $"{Common.DataPath}\\fsflightbuildertmp.db", true);
                    Common.DBConnName = "FSFBDbConnTmp";
                }
                else if (MessageBox.Show(@"The FS Flight Builder database could not be backed up. Do you want to continue?", @"Database Backup Failed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    return;
                }

                //Fill the list of aircraft thumbnail folders
                Common.GetAllAircraft(ref aircraftFiles);
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Error during database update. Error is {ex.Message}");
            }

            factor = 100.0 / model.MaxFiles;
            pbImporter.Visible = true;
            btnCancel.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var message = "Are you sure you want to cancel the database update process?";
            var lbl = "Database Update In Progress";
            if (MessageBox.Show(message, lbl, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
            {
                return;
            }

            Common.logger.Info($"Cancelling the database update.");
            // Create a copy of the event to work with
            EventHandler<EventArgs> ea = Canceled;
            ea?.Invoke(this, e);

            if (backgroundWorker1.WorkerSupportsCancellation)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
                try
                {
                    SqliteConnection.ClearAllPools();
                    MessageBox.Show(@"Operation cancelled.");
                    if (File.Exists($"{Common.DataPath}\\fsflightbuildertmp.db"))
                    {
                        File.Delete($"{Common.DataPath}\\fsflightbuildertmp.db");
                    }
                    if (File.Exists($"{Common.DataPath}\\navdata.db"))
                    {
                        File.Delete($"{Common.DataPath}\\navdata.db");
                    }
                }
                catch
                {
                    Console.Write("Could not delete temp database.");
                    Common.logger.Error($"Attempted to delete the temp database(s) and failed.");
                }
                finally
                {
                    Common.DBConnName = "FSFBDbConn";

                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            try
            {
                using (var context = new navDataReaderDbConn(Common.DataPath))
                {
                    ParseAirports(context, worker);
                    ParseNavaids(context, worker);
                    ParseWaypoints(context, worker);
                    ParseAirways(context, worker);
                    ParseAircraft(worker);
                }

                model.Saving = true;
                model.MaxFiles =
                    waypoints.Count +
                    airports.Count +
                    runways.Count +
                    comms.Count +
                    parkings.Count +
                    aircraftFiles.Count;
                model.Progress = 0;
                model.PctProgress = 0;
                oldPercentage = 0;
                factor = 100.0 / model.MaxFiles;
                SaveData(worker);
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Error during database update main process. Error is {ex.Message}");
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                var model = (ProgressReportModel)e.UserState;
                lblAirports.Text = $"{model.AirportCount:n0}";
                lblRunways.Text = $"{model.RunwayCount:n0}";
                lblComms.Text = $"{model.CommCount:n0}";
                lblParkings.Text = $"{model.ParkingCount:n0}";
                lblNavaids.Text = $"{model.NavaidCount:n0}";
                lblWaypoints.Text = $"{model.WaypointsCount:n0}";
                lblRoutes.Text = $"{model.RoutesCount:n0}";
                if (model.Aircraft.Count > 0)
                {
                    lstAircraft.DataSource = model.Aircraft;
                }

                if (model.PctProgress > 100)
                {
                    model.PctProgress = 100;
                }

                if (model.Caption.StartsWith("Updating Database"))
                {
                    pbImporter.CustomText = model.Caption;
                    pbImporter.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    pbImporter.CustomText = model.Caption + " (" + pbImporter.Value + "%)";
                    pbImporter.Style = ProgressBarStyle.Blocks;
                    pbImporter.Value = model.PctProgress;
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCancel.Enabled = false;
            pbImporter.UseWaitCursor = false;
            pbImporter.Visible = false;

            Common.logger.Info($"Database update task complete.");
            if (File.Exists($"{Common.DataPath}\\fsflightbuildertmp.db") && File.Exists($"{Common.DataPath}\\fsflightbuilder.db"))
            {
                SqliteConnection.ClearAllPools();
                try
                {
                    //Rename fsflightbuilder.db
                    File.Move($"{Common.DataPath}\\fsflightbuilder.db", $"{Common.DataPath}\\fsflightbuilder.db.bak");
                    File.Move($"{Common.DataPath}\\fsflightbuildertmp.db", $"{Common.DataPath}\\fsflightbuilder.db");
                    File.Delete($"{Common.DataPath}\\fsflightbuilder.db.bak");

                    var dbPath = $"{Common.DataPath}\\navdata.db";
                    FileInfo fi = new FileInfo(dbPath);
                    try
                    {
                        if (fi.Exists)
                        {
                            SqliteConnection connection = new SqliteConnection("Data Source=" + dbPath + ";");
                            connection.Close();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            SqliteConnection.ClearAllPools();
                            fi.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.logger.Error($"Error deleting the navdata file. Error is {ex.Message}");
                        SqliteConnection.ClearAllPools();
                        try
                        {
                            fi.Delete();
                        }
                        catch { }
                    }
                    MessageBox.Show($"Database update completed successfully. Click OK to continue.", $"Database Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occured while updating the database. Error is {ex.Message}", $"Database Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Abort;
                }
                finally
                {
                    Common.DBConnName = "FSFBDbConn";
                    Close();
                }
            }
        }

        private void ClearLists()
        {
            airports.Clear();
            waypoints.Clear();
            runways.Clear();
            parkings.Clear();
            comms.Clear();
            aircraftFiles.Clear();
            lstAircraft.Items.Clear();
        }

        private void ParseAirports(navDataReaderDbConn ctx, BackgroundWorker worker)
        {
            Common.logger.Info($"Parsing Airports.");
            model.Caption = "Almost there. Compiling airport data. Please wait...";
            model.AirportCount = 0;
            model.RunwayCount = 0;
            model.CommCount = 0;
            model.ParkingCount = 0;
            model.Progress = 0;
            worker?.ReportProgress(model.Progress, model);
            try
            {
                //Read Airports
                var dbairports = ctx.Airports
                    .Include(r => r.Runways)
                    .ThenInclude(re => re.PrimaryEnd)
                    .ThenInclude(i => i.Ils)
                    .Include(r => r.Runways)
                    .ThenInclude(re => re.SecondaryEnd)
                    .ThenInclude(i => i.Ils)
                    .Include(c => c.Comms)
                    .Include(p => p.Parkings).ToList();

                model.MaxFiles = dbairports.Count;
                factor = 100.0 / dbairports.Count;

                foreach (var airport in dbairports)
                {
                    try
                    {
                        model.Progress++;
                        airports.Add(new Data.Models.Airport
                        {
                            FSType = (int)Common.FlightSim,
                            AirportId = airport.Ident,
                            IcaoName = airport.Name,
                            Elevation = airport.Altitude,
                            Latitude = airport.Laty,
                            Longitude = airport.Lonx,
                            MagVar = airport.MagVar,
                            Country = airport.Country,
                            State = airport.State,
                            City = airport.City,
                            HasTower = airport.TowerFrequency != null && airport.TowerFrequency > 0 ? 1 : 0,
                            HasAvGas = Convert.ToInt32(airport.HasAvgas),
                            HasJetFuel = Convert.ToInt32(airport.HasJetfuel),
                            LongestRwyLength = Convert.ToInt32(airport.LongestRunwayLength),
                            LongestRwyWidth = Convert.ToInt32(airport.LongestRunwayWidth)
                        });
                        model.Caption = "Reading Airports";
                        model.AirportCount = airports.Count;
                    }
                    catch (Exception ex)
                    {
                        Common.logger.Error($"Error generating airport collection. Error is {ex.Message}");
                    }

                    try
                    {
                        foreach (var runway in airport.Runways)
                        {
                            var dbPrimaryEnd = runway.PrimaryEnd;
                            var dbSecondaryEnd = runway.SecondaryEnd;
                            var dbPrimaryILS = dbPrimaryEnd.Ils.ToList();
                            var dbSecondaryILS = dbSecondaryEnd.Ils.ToList();
                            var divisor = dbPrimaryILS.Count > 0 ? dbPrimaryILS[0].Frequency > 999999 ? 1000000 : 1000 : 1000;
                            //Add the primary runway
                            runways.Add(new Data.Models.Runway
                            {
                                FSType = (int)Common.FlightSim,
                                AirportId = runway.Airport != null ? runway.Airport.Ident : string.Empty,
                                Number = dbPrimaryEnd.Name,
                                Length = (long)runway.Length,
                                Width = (long)runway.Width,
                                Latitude = dbPrimaryEnd.Laty,
                                Longitude = dbPrimaryEnd.Lonx,
                                Heading = Math.Round((decimal)dbPrimaryEnd.Heading, 2),
                                Surface = runway.Surface,
                                IlsFrequency = dbPrimaryILS.Count > 0 ? (Convert.ToDecimal(dbPrimaryILS[0].Frequency) / divisor).ToString() : string.Empty,
                                IlsHeading = dbPrimaryILS.Count > 0 ? Math.Round((decimal)dbPrimaryILS[0].LocHeading, 2) : 0,
                                ApproachLights = dbPrimaryEnd.AppLightSystemType,
                                Glideslope = dbPrimaryEnd.LeftVasiPitch != null ? dbPrimaryEnd.LeftVasiPitch.ToString() : dbPrimaryEnd.RightVasiPitch.ToString(),
                                PatternTakeoff = dbPrimaryEnd.IsPattern == "L" ? "Left" : "Right",
                                PatternLanding = dbPrimaryEnd.IsPattern == "L" ? "Left" : "Right",
                                PatternAltitude = runway.PatternAltitude.ToString()
                            });
                            //Add the secondary runway
                            runways.Add(new Data.Models.Runway
                            {
                                FSType = (int)Common.FlightSim,
                                AirportId = runway.Airport != null ? runway.Airport.Ident : string.Empty,
                                Number = dbSecondaryEnd.Name,
                                Length = (long)runway.Length,
                                Width = (long)runway.Width,
                                Latitude = dbSecondaryEnd.Laty,
                                Longitude = dbSecondaryEnd.Lonx,
                                Heading = Math.Round((decimal)dbSecondaryEnd.Heading, 2),
                                Surface = runway.Surface,
                                IlsFrequency = dbSecondaryILS.Count > 0 ? (Convert.ToDecimal(dbSecondaryILS[0].Frequency) / divisor).ToString() : string.Empty,
                                IlsHeading = dbSecondaryILS.Count > 0 ? Math.Round((decimal)dbSecondaryILS[0].LocHeading, 2) : 0,
                                ApproachLights = dbSecondaryEnd.AppLightSystemType,
                                Glideslope = dbSecondaryEnd.LeftVasiPitch != null ? dbSecondaryEnd.LeftVasiPitch.ToString() : dbSecondaryEnd.RightVasiPitch.ToString(),
                                PatternTakeoff = dbPrimaryEnd.IsPattern == "L" ? "Left" : "Right",
                                PatternLanding = dbPrimaryEnd.IsPattern == "L" ? "Left" : "Right",
                                PatternAltitude = runway.PatternAltitude.ToString()
                            });
                        }
                        model.RunwayCount += airport.Runways.Count;
                    }
                    catch (Exception ex)
                    {
                        Common.logger.Error($"Error generating runway collection. Error is {ex.Message}");
                    }

                    //Read Parking Spots
                    try
                    {
                        foreach (var park in airport.Parkings.Distinct())
                        {
                            parkings.Add(new Data.Models.Parking
                            {
                                FSType = (int)Common.FlightSim,
                                AirportId = park.Airport.Ident,
                                Type = ParkingUtil.parkingTypeToStr(park.Type),
                                Name = park.Name,
                                Number = park.Number,
                                Latitude = park.Laty,
                                Longitude = park.Lonx,
                                Heading = Convert.ToInt64(park.Heading)
                                //GateType = park.parking_type
                            });
                        }
                        model.ParkingCount = parkings.Count;
                    }
                    catch (Exception ex)
                    {
                        Common.logger.Error($"Error generating parking collection. Error is {ex.Message}");
                    }

                    //Read Comms
                    try
                    {
                        foreach (var comm in airport.Comms)
                        {
                            var strCommType = CommUtil.commTypeToStr(comm.Type);
                            CommType enumCommType;
                            Enum.TryParse<CommType>(strCommType, out enumCommType);
                            var divisor = comm.Frequency > 999999 ? 1000000 : 1000;
                            comms.Add(new Data.Models.Comm
                            {
                                FSType = (int)Common.FlightSim,
                                AirportId = comm.Airport.Ident,
                                Type = Convert.ToInt32(enumCommType),
                                Frequency = (Convert.ToDecimal(comm.Frequency) / divisor).ToString(),
                                Name = comm.Name
                            });
                        }
                        model.CommCount = comms.Count;
                        ReportProgress(model.Progress, worker);
                    }
                    catch (Exception ex)
                    {
                        Common.logger.Error($"Error generating comm collection. Error is {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Error getting dataset from Airports. Error is {ex.Message}");
            }
        }

        private void ParseNavaids(navDataReaderDbConn ctx, BackgroundWorker worker)
        {
            Common.logger.Info($"Parsing VORs");
            //Read VORs
            var dbvors = ctx.VORs.ToList();

            try
            {
                model.Caption = "Reading Navaids";
                model.MaxFiles = dbvors.Count;
                factor = 100.0 / dbvors.Count;
                model.NavaidCount = 0;
                model.Progress = 0;
                oldPercentage = 0;
                ReportProgress(model.Progress, worker);
                foreach (var vor in dbvors)
                {
                    model.Progress++;
                    var divisor = vor.Frequency > 999999 ? 1000000 : 1000;
                    navaids.Add(new Navaid
                    {
                        FSType = (int)Common.FlightSim,
                        NavId = vor.Ident,
                        Name = vor.Name,
                        Frequency = (Convert.ToDecimal(vor.Frequency) / divisor).ToString(),
                        Latitude = vor.Laty,
                        Longitude = vor.Lonx,
                        Elevation = vor.Altitude ?? 0,
                        Type = vor.Type,
                        Region = vor.Region,
                        MagVar = vor.MagVar
                    });
                    model.Caption = "Reading Navaids";
                    model.NavaidCount = navaids.Count;
                    ReportProgress(model.Progress, worker);
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Error generating vor collection. Error is {ex.Message}");
            }

            Common.logger.Info($"Parsing NDBs");
            //Read NDBs
            try
            {
                var dbndbs = ctx.NDBs.ToList();

                model.Caption = "Reading Navaids";
                model.MaxFiles = dbndbs.Count + dbvors.Count;
                factor = 100.0 / (dbndbs.Count + dbvors.Count);
                oldPercentage = 50;
                ReportProgress(model.Progress, worker);
                foreach (var ndb in dbndbs)
                {
                    model.Progress++;
                    var divisor = ndb.Frequency > 999999 ? 1000000 : 1000;
                    navaids.Add(new Navaid
                    {
                        FSType = (int)Common.FlightSim,
                        NavId = ndb.Ident,
                        Name = ndb.Name,
                        Frequency = (Convert.ToDecimal(ndb.Frequency) / divisor).ToString(),
                        Latitude = ndb.Laty,
                        Longitude = ndb.Lonx,
                        Elevation = ndb.Altitude ?? 0,
                        Type = ndb.Type,
                        Region = ndb.Region,
                        MagVar = ndb.MagVar
                    });
                    model.Caption = "Reading Navaids";
                    model.NavaidCount = navaids.Count;
                    ReportProgress(model.Progress, worker);
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Error generating ndb collection. Error is {ex.Message}");
            }
        }

        private void ParseWaypoints(navDataReaderDbConn ctx, BackgroundWorker worker)
        {
            Common.logger.Info($"Parsing waypoints");
            //Read Waypoints
            try
            {
                dbwaypoints = ctx.Waypoints.ToList();

                model.Caption = "Reading Waypoints";
                model.MaxFiles = dbwaypoints.Count;
                model.Progress = 0;
                oldPercentage = 0;
                factor = 100.0 / dbwaypoints.Count;
                ReportProgress(model.Progress, worker);
                foreach (var waypoint in dbwaypoints)
                {
                    model.Progress++;
                    waypoints.Add(new Data.Models.Waypoint
                    {
                        FSType = (int)Common.FlightSim,
                        //WaypointId = waypoint.WaypointId,
                        NavId = waypoint.Ident,
                        Latitude = waypoint.Laty,
                        Longitude = waypoint.Lonx,
                        Type = waypoint.Type,
                        Region = waypoint.Region,
                        MagVar = waypoint.MagVar
                    });
                    model.Caption = "Reading Waypoints";
                    model.WaypointsCount = waypoints.Count;
                    ReportProgress(model.Progress, worker);
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Error generating waypoints collection. Error is {ex.Message}");
            }
        }

        private void ParseAirways(navDataReaderDbConn ctx, BackgroundWorker worker)
        {
            Common.logger.Info($"Parsing airways");
            //Read Airways/Routes
            try
            {
                dbairways = ctx.Airways.ToList();

                model.Caption = "Reading Routes";
                model.MaxFiles = dbairways.Count;
                model.Progress = 0;
                oldPercentage = 0;
                factor = 100.0 / dbairways.Count;
                ReportProgress(model.Progress, worker);
                foreach (var airway in dbairways)
                {
                    model.Progress++;
                    string rteType;
                    switch (airway.AirwayType)
                    {
                        case "V":
                            rteType = "VICTOR";
                            break;
                        case "J":
                            rteType = "JET";
                            break;
                        case "B":
                            rteType = "BOTH";
                            break;
                        default:
                            rteType = "NONE";
                            break;
                    }
                    routes.Add(new Data.Models.Route
                    {
                        FSType = (int)Common.FlightSim,
                        Name = airway.AirwayName,
                        WaypointNavId = airway.AirwayName,
                        Type = rteType,
                        Next = airway.ToWaypoint.Ident,
                        Previous = airway.FromWaypoint.Ident
                    });
                    model.Caption = "Reading Routes";
                    model.RoutesCount = routes.Count;
                    ReportProgress(model.Progress, worker);
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Error generating airway collection. Error is {ex.Message}");
            }
        }

        private void ParseAircraft(BackgroundWorker worker)
        {
            Common.logger.Info("Parsing aircraft");
            try
            {
                model.Caption = "Reading Aircraft";
                model.Aircraft = new List<string>();
                model.Progress = 0;
                oldPercentage = 0;
                ReportProgress(model.Progress, worker);

                aircraft = Common.ParseAircraft(aircraftFiles);
                foreach (var acft in aircraft)
                {
                    var name = acft.Name;
                    Common.logger.Info($"Aircraft1: {name}");
                    var idx = acft.Name.IndexOf(" (");
                    try
                    {
                        name = idx > -1 ? acft.Name.Substring(0, idx) : acft.Name;
                        Common.logger.Info($"Aircraft: {name}");
                    }
                    catch
                    {
                        Common.logger.Error($"Aircraft error: {name}");
                    }
                    if (!model.Aircraft.Contains(name))
                    {
                        model.Aircraft.Add(name);
                    }
                }
                factor = 100.0 / aircraft.Count;

                model.Caption = "Reading Aircraft Complete";
                model.Progress = 100;
                ReportProgress(model.Progress, worker);
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Error parsing aircraft. Error is: {ex.Message}");
            }
        }

        internal bool SaveData(BackgroundWorker worker)
        {
            Common.logger.Info($"Saving data.");
            var dbAircraft = new List<Aircraft>();

            model.Caption = "Compiling Data";
            ReportProgress(0, worker);

            //Open the tmp database
            using (var context = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                try
                {
                    Common.logger.Info($"Removing FS Data");
                    RemoveFSData(context, worker);
                }
                catch (Exception ex)
                {
                    Common.logger.Error($"Error removing FS Data. Error is {ex.Message}");
                }
                for (int i = 0; i < 8; i++)
                {
                    switch (i)
                    {
                        case 0: //Airports
                            try
                            {
                                Common.logger.Info($"Saving Airports.");
                                model.Caption = "Updating Database - Airports";
                                worker?.ReportProgress(model.PctProgress, model);

                                if (!worker.CancellationPending)
                                {
                                    using (var transaction = context.Database.BeginTransaction())
                                    {
                                        context.Airports.AddRange(airports);
                                        context.ChangeTracker.DetectChanges();
                                        context.SaveChanges();
                                        transaction.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Common.logger.Error($"Error saving airports. Error is {ex.Message}");
                            }
                            break;
                        case 1: //Runways
                            try
                            {
                                Common.logger.Info($"Saving runways");
                                model.Caption = "Updating Database - Runways";
                                worker?.ReportProgress(model.PctProgress, model);
                                if (!worker.CancellationPending)
                                {
                                    using (var transaction = context.Database.BeginTransaction())
                                    {
                                        context.Runways.AddRange(runways);
                                        context.ChangeTracker.DetectChanges();
                                        context.SaveChanges();
                                        transaction.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Common.logger.Error($"Error saving runways. Error is {ex.Message}");
                            }
                            break;
                        case 2: //Parkings
                            try
                            {
                                Common.logger.Info($"Saving parking spots");
                                model.Caption = "Updating Database - Parking Spots";
                                worker?.ReportProgress(model.PctProgress, model);
                                if (!worker.CancellationPending)
                                {
                                    context.Parkings.AddRange(parkings);
                                    context.ChangeTracker.DetectChanges();
                                    context.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                Common.logger.Error($"Error saving parkings. Error is {ex.Message}");
                            }
                            break;
                        case 3: //Comms
                            try
                            {
                                Common.logger.Info($"Saving comms");
                                model.Caption = "Updating Database - Comms";
                                worker?.ReportProgress(model.PctProgress, model);
                                if (!worker.CancellationPending)
                                {
                                    using (var transaction = context.Database.BeginTransaction())
                                    {
                                        context.Comms.AddRange(comms);
                                        context.ChangeTracker.DetectChanges();
                                        context.SaveChanges();
                                        transaction.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Common.logger.Error($"Error saving comms. Error is {ex.Message}");
                            }
                            break;
                        case 4: //Aircraft
                            try
                            {
                                Common.logger.Info($"Updating aircraft. aircraft count: {aircraft.Count}");
                                model.Caption = "Updating Database - Aircraft";
                                worker?.ReportProgress(model.PctProgress, model);
                                if (!worker.CancellationPending)
                                {
                                    using (var transaction = context.Database.BeginTransaction())
                                    {
                                        aircraft = Common.FixAircraftData(context, aircraft);
                                        //Remove duplicates
                                        //var noAircraftDups = aircraft.GroupBy(a => new { a.Name, a.Texture, a.FSType })
                                        //                          //.Where(a => a.Count() == 1)
                                        //                          .Select(a => a.First())
                                        //                          .ToList();
                                        context.Aircraft.AddRange(aircraft); // noAircraftDups);
                                        context.ChangeTracker.DetectChanges();
                                        context.SaveChanges();
                                        transaction.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Common.logger.Error($"Error saving aircraft. Error is {ex.Message}");
                            }
                            break;
                        case 5: //VOR
                            try
                            {
                                Common.logger.Info($"Saving Vors");
                                model.Caption = "Updating Database - Navaids";
                                worker?.ReportProgress(model.PctProgress, model);
                                if (!worker.CancellationPending)
                                {
                                    using (var transaction = context.Database.BeginTransaction())
                                    {
                                        context.Navaids.AddRange(navaids);
                                        context.ChangeTracker.DetectChanges();
                                        context.SaveChanges();
                                        transaction.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Common.logger.Error($"Error saving navaids. Error is {ex.Message}");
                            }
                            break;
                        case 6: //Waypoint
                            Common.logger.Info($"Saving waypoints");
                            model.Caption = "Updating Database - Waypoints";
                            worker?.ReportProgress(model.PctProgress, model);
                            if (!worker.CancellationPending)
                            {
                                using (var transaction = context.Database.BeginTransaction())
                                {
                                    context.Waypoints.AddRange(waypoints);
                                    context.ChangeTracker.DetectChanges();
                                    context.SaveChanges();
                                    transaction.Commit();
                                }
                            }
                            break;
                        case 7: //Routes
                            try
                            {
                                Common.logger.Info($"Saving routes");
                                model.Caption = "Updating Database - Routes";
                                worker?.ReportProgress(model.PctProgress, model);
                                if (!worker.CancellationPending)
                                {
                                    using (var transaction = context.Database.BeginTransaction())
                                    {
                                        context.Routes.AddRange(routes);
                                        context.ChangeTracker.DetectChanges();
                                        context.SaveChanges();
                                        transaction.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Common.logger.Error($"Error saving routes. Error is {ex.Message}");
                            }
                            break;
                    }
                }
            }
            model.PctProgress = 100;
            model.Caption = "Updating Database - Complete";
            worker?.ReportProgress(100, model);
            return true;
        }

        private void RemoveFSData(FSFBDbConn context, BackgroundWorker worker)
        {
            var percentage = 12;
            model.Caption = "Compiling Data - Aircraft";
            ReportProgress(0, worker);
            try
            {
                context.Database.ExecuteSql($"DELETE FROM [Aircraft] WHERE FSType = {(int)Common.FlightSim}");
                context.SaveChanges();
            }
            catch { }
            model.Caption = "Compiling Data - Comms";
            ReportProgress(percentage * 1, worker);
            try
            {
                context.Database.ExecuteSql($"DELETE FROM [Comms] WHERE FSType = {(int)Common.FlightSim}");
                context.SaveChanges();
            }
            catch { }
            model.Caption = "Compiling Data - Parking Spots";
            ReportProgress(percentage * 2, worker);
            try
            {
                context.Database.ExecuteSql($"DELETE FROM [Parkings] WHERE FSType = {(int)Common.FlightSim}");
                context.SaveChanges();
            }
            catch { }
            model.Caption = "Compiling Data - Runways";
            ReportProgress(percentage * 3, worker);
            try
            {
                context.Database.ExecuteSql($"DELETE FROM [Runways] WHERE FSType = {(int)Common.FlightSim}");
                context.SaveChanges();
            }
            catch { }
            model.Caption = "Compiling Data - Navaids";
            ReportProgress(percentage * 4, worker);
            try
            {
                context.Database.ExecuteSql($"DELETE FROM [Navaids] WHERE FSType = {(int)Common.FlightSim}");
                context.SaveChanges();
            }
            catch { }
            model.Caption = "Compiling Data - Routes";
            ReportProgress(percentage * 5 + 1, worker);
            try
            {
                context.Database.ExecuteSql($"DELETE FROM [Routes] WHERE FSType = {(int)Common.FlightSim}");
                context.SaveChanges();
            }
            catch { }
            model.Caption = "Compiling Data - Waypoints";
            ReportProgress(percentage * 6 + 1, worker);
            try
            {
                context.Database.ExecuteSql($"DELETE FROM [Waypoints] WHERE FSType = {(int)Common.FlightSim}");
                context.SaveChanges();
            }
            catch { }
            model.Caption = "Compiling Data - Airports";
            ReportProgress(percentage * 7 + 1, worker);
            try
            {
                context.Database.ExecuteSql($"DELETE FROM [Airports] WHERE FSType = {(int)Common.FlightSim}");
                context.SaveChanges();
            }
            catch { }
            model.Caption = "Compiling Data - Complete";
            ReportProgress(100, worker);
        }

        private void ReportProgress(int count, BackgroundWorker worker)
        {
            var percentage = (int)(count * factor);
            if (percentage > oldPercentage)
            {
                if (percentage < 100 || !model.Saving)
                {
                    oldPercentage = percentage;
                    model.PctProgress = percentage;
                    worker?.ReportProgress(percentage, model);
                }
            }
        }

        private void BGLImporter_Shown(object sender, EventArgs e)
        {
            //Start the NavDataReader app
            var appPath = Path.GetDirectoryName(Application.ExecutablePath);
            var navDataReaderPath = Path.Combine(appPath, $"Data\\NavDataReader\\navdatareader.exe");
            var ars = string.Empty;
            switch (Common.FlightSim)
            {
                case FlightSimType.MSFS:
                    ars = $"-f MSFS";
                    break;
                case FlightSimType.P3D:
                    var path = Common.flightSimPaths[Common.FlightSim].DefaultFSPath;
                    var idx = path.ToUpper().LastIndexOf('V');
                    var version = idx > -1 ? path.Substring(idx, 2) : "V2";
                    ars = $"-f P3D{version.ToUpper()}";
                    break;
                case FlightSimType.FSXSE:
                    if (Common.flightSimPaths[FlightSimType.FSXSE].DefaultFSPath.ToLower().Contains("fsx-se"))
                    {
                        ars = $"-f FSXSE";
                    }
                    else
                    {
                        ars = $"-f FSX";
                    }
                    break;
                case FlightSimType.FSX:
                    ars = $"-f FSX";
                    break;
            }

            if (!string.IsNullOrEmpty(ars))
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = navDataReaderPath;
                processStartInfo.Arguments = ars;

                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;

                Process process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();

                waitForm.ProcessName = process.ProcessName;
                waitForm.TopLabel = "Gathering your flight simulator data.";
                waitForm.CenterLabel = "This may take several minutes.";
                waitForm.Show(this);

                process.WaitForExit();
            }

            if (waitForm.dlgResult == DialogResult.OK)
            {
                waitForm.Close();
                //Open the navdatareader database
                if (File.Exists($"{appPath}\\navdata.sqlite"))
                {
                    File.Move($"{appPath}\\navdata.sqlite", $"{Common.DataPath}\\navdata.db", true);
                }
                else if (File.Exists($"{appPath}\\navdata_BROKEN.sqlite"))
                {
                    File.Move($"{appPath}\\navdata_BROKEN.sqlite", $"{Common.DataPath}\\navdata.db", true);
                }
                Common.DBConnName = "FSFBDbConnTmp";


                //Set up the progress bar
                pbImporter.UseWaitCursor = true;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                this.Close();
            }
        }
    }
}
