using BGLParser.logging;
using FSFlightBuilder.Components.FlightSims;
using FSFlightBuilder.Data.Models;
using FSFlightBuilder.Entities;
using FSFlightBuilder.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;
using RTB_ToolTip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static FSFlightBuilder.Components.TOCTOD_Helpers;

namespace FSFlightBuilder.Components
{
    internal static class Common
    {
        public static string appPath { get; set; }
        public static string fsxAppVersion { get; set; }
        public static string fsxSEAppVersion { get; set; }
        public static string p3dAppVersion { get; set; }
        public static string msfsAppVersion { get; set; }
        public static FlightSimType FlightSim { get; set; }
        public static FlightSimType PriorFlightSim { get; set; }
        public static bool EmailOk { get; set; }
        public static string ChartService { get; set; }
        public static bool ChartNotification { get; set; }
        public static string AIRAC { get; set; }
        public static bool deleteImages = true;
        public static string pf3Path = string.Empty;
        public static List<Airport> AllAirports { get; set; }
        public static bool DataUpdateInProgress = false;
        public static string DataPath = string.Empty;
        public static ILogger logger = LogManager.GetCurrentClassLogger();
        public static Dictionary<FlightSimType, FSPaths> flightSimPaths;
        public static readonly string FSX_REGISTRY_PATH = "HKEY_CURRENT_USER\\Software\\Microsoft";
        public static readonly string[] FSX_REGISTRY_KEY = { "Microsoft Games", "Flight Simulator", "10.0", "AppPath" };
        public static string DBConnName = "FSFBDbConn";

        public static string C2Dms(double coord, bool isLat, bool shorter = false)
        {
            var dir = isLat ? (coord >= 0 ? "N" : "S") : (coord >= 0 ? "E" : "W");
            coord = Math.Abs(coord);
            var minPart = ((coord - Math.Truncate(coord) / 1) * 60);
            var secPart = !shorter ? ((minPart - Math.Truncate(minPart) / 1) * 60).ToString("N6") : ((minPart - Math.Truncate(minPart) / 1) * 60).ToString("N2");
            return dir + Math.Truncate(coord) + "° " + Math.Truncate(minPart) + "' " + secPart + "\"";
        }

        public static string LatLon(double? coord, bool isLat)
        {
            var dir = isLat ? (coord >= 0 ? "N" : "S") : (coord >= 0 ? "E" : "W");
            coord = Math.Abs((double)coord);
            var minPart = ((coord - Math.Truncate((double)coord) / 1) * 60);
            var secPart = Convert.ToDecimal(((minPart - Math.Truncate((double)minPart) / 1) * 60)).ToString("N2");
            return dir + Math.Truncate((double)coord) + @"&#xB0; " + Math.Truncate((double)minPart) + @"&apos; " + secPart + @"&quot;";
        }

        public static string LatLon(double coord, bool isLat, bool isFlt)
        {
            var dir = isLat ? (coord >= 0 ? "N" : "S") : (coord >= 0 ? "E" : "W");
            coord = Math.Abs(coord);
            var minPart = (coord - Math.Truncate(coord) / 1) * 60;
            var secPart = (minPart - Math.Truncate(minPart) / 1) * 60;
            return dir + Math.Truncate(coord) + "° " + (Math.Truncate(minPart) + secPart / 60).ToString("N2") + "'";
        }

        public static void ConfigureLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logPath = DataPath.Replace("\\Data", "\\Logs\\");
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            config.Variables.Add("LogHome", logPath);
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"{logPath}\\fsflightbuilder.log", Layout = "${longdate}|${callsite}|${message}|${exception}", ArchiveFileName = "${LogHome}/Archive/Info-${shortdate}.awd", MaxArchiveFiles = 5, ArchiveEvery = FileArchivePeriod.Day };
            var errorlog = new NLog.Targets.FileTarget("errorlog") { FileName = $"{logPath}\\fsflightbuilder_error.log", Layout = "${longdate}|${callsite}|${message}|${exception}", ArchiveFileName = "${LogHome}/Archive/Error-${shortdate}.awd", MaxArchiveFiles = 5, ArchiveEvery = FileArchivePeriod.Day };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Error, LogLevel.Fatal, errorlog);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }

        internal static StartupOptions SimStartup()
        {
            var opts = new StartupOptions();
            appPath = Path.GetDirectoryName(Application.ExecutablePath);
            logger.Info("Getting Flight Sim settings.");
            GetAllSettings();
            logger.Info("1. Flight Sim = " + FlightSim);
            logger.Info("Updating Sim versions.");
            UpdateSimVersions();

            if (string.IsNullOrEmpty(ChartService) || ChartService != "FAA")
            {
                ChartService = "AVIATIONAPI";
                Properties.Settings.Default.Service = "AVIATIONAPI";
            }

            if (!ChartNotification && ChartService == "AIRCHARTS")
            {
                MessageBox.Show(@"The https://www.aircharts.org chart service may be down.  If you experience issues with charts, and are flying within the United States, please choose the FAA Charts option from the File:Options menu item.", "AirCharts Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Properties.Settings.Default.ChartNotification = true;
            }
            Properties.Settings.Default.Save();
            logger.Info("4. Flight Sim = " + FlightSim);

            logger.Info("Checking default sim.");

            logger.Info("5. Flight Sim = " + FlightSim);
            if (FlightSim == FlightSimType.Unknown)
            {
                //Check the FS Paths
                var pathFound = false;
                FlightSimType fs = FlightSimType.Unknown;
                for (var i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            fs = FlightSimType.FSX;
                            break;
                        case 1:
                            fs = FlightSimType.FSXSE;
                            break;
                        case 2:
                            fs = FlightSimType.P3D;
                            break;
                        case 3:
                            fs = FlightSimType.MSFS;
                            break;
                    }
                    if (!string.IsNullOrEmpty(flightSimPaths[fs].AppDataPath) && !string.IsNullOrEmpty(flightSimPaths[fs].DefaultFSPath))
                    {
                        pathFound = true;
                        break;
                    }
                }

                if (pathFound)
                {
                    MessageBox.Show(@"The flight sim could not be determined.  Please select the default flight simulator from the Options screen.",
                        @"Flight Sim Not Identified", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    var frm = new FrmOptions();
                    frm.ShowDialog();
                    if (string.IsNullOrEmpty(flightSimPaths[FlightSim].DefaultFSPath))
                    {
                        logger.Error($"Path is found but DefaultFSPath for {FlightSim} is not found.");
                        return null;
                    }
                }
                else
                {
                    //try to get the installed flight sims
                    if (CheckInstalls() == string.Empty)
                    {

                        logger.Error("Error, cannot detect flight sim.");
                        MessageBox.Show(@"FS Flight Builder still cannot determine your installed Flight Sim.  Please re-launch the application and try again." +
                            Environment.NewLine + Environment.NewLine + "If you believe this is an error, please contact the developer at tarduini@arduiniwebdevelopment.com",
                             @"Cannot Determine Flight Sim", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (File.Exists(appPath + @"\FSFlightBuilder.ini"))
                        {
                            File.Delete(appPath + @"\FSFlightBuilder.ini");
                        }
                        Environment.Exit(-1);
                    }
                }
            }

            if (!Directory.Exists(flightSimPaths[FlightSim].DefaultFSPath))
            {
                MessageBox.Show($"The flight sim path ({flightSimPaths[FlightSim].DefaultFSPath}) for {FlightSim} could not be found.", "Flight Sim Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            opts.LastFlightTime = Properties.Settings.Default.LastFlight;
            opts.FlightDate = AWDConvert.ToDateTime(opts.LastFlightTime);
            if (opts.FlightDate == DateTime.MinValue)
            {
                opts.FlightDate = DateTime.Now;
            }

            // If P3D v4 or higher, change weather types to Theme and User Defined
            opts.WeatherTypes = new List<string>();
            if (FlightSim == FlightSimType.P3D)
            {
                var ver = p3dAppVersion.Split('.');
                if (ver.Length > 1 && AWDConvert.ToInt32(ver[0]) > 3 && AWDConvert.ToInt32(ver[1]) > 4)
                {
                    opts.WeatherTypes.Add("Theme");
                    opts.WeatherTypes.Add("User Defined");
                }

            }
            else if (FlightSim == FlightSimType.MSFS)
            {
                opts.WeatherTypes.Add("Defined in the Sim");
            }

            //TODO: TOC Update
            opts.IncludeTOD = Properties.Settings.Default.IncludeTOD;
            return opts;
        }

        internal static void UpdateSimVersions()
        {
            if (flightSimPaths != null)
            {
                if (flightSimPaths.ContainsKey(FlightSimType.FSX) && File.Exists(flightSimPaths[FlightSimType.FSX] + @"\fsx.exe"))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(flightSimPaths[FlightSimType.FSX] + @"\fsx.exe");
                    var idx = versionInfo.ProductVersion.IndexOf(" ", StringComparison.Ordinal);
                    var version = idx > -1
                        ? versionInfo.ProductVersion.Substring(0, idx)
                        : versionInfo.ProductVersion;
                    if (version != fsxAppVersion)
                    {
                        fsxAppVersion = version;
                        Properties.Settings.Default.FSXVersion = version;

                    }
                }
                if (flightSimPaths.ContainsKey(FlightSimType.FSXSE) && File.Exists(flightSimPaths[FlightSimType.FSXSE] + @"\fsx.exe"))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(flightSimPaths[FlightSimType.FSXSE] + @"\fsx.exe");
                    var idx = versionInfo.ProductVersion.IndexOf(" ", StringComparison.Ordinal);
                    var version = idx > -1
                            ? versionInfo.ProductVersion.Substring(0, idx)
                            : versionInfo.ProductVersion;
                    if (version != fsxSEAppVersion)
                    {
                        fsxSEAppVersion = version;
                        Properties.Settings.Default.FSXSEVersion = version;
                    }
                }
                if (flightSimPaths.ContainsKey(FlightSimType.P3D) && File.Exists(flightSimPaths[FlightSimType.P3D] + @"\Prepar3D.exe"))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(flightSimPaths[FlightSimType.P3D] + @"\Prepar3D.exe");
                    var idx = versionInfo.ProductVersion.IndexOf(" ", StringComparison.Ordinal);
                    var version = idx > -1
                        ? versionInfo.ProductVersion.Substring(0, idx)
                        : versionInfo.ProductVersion;
                    if (version != p3dAppVersion)
                    {
                        p3dAppVersion = version;
                        Properties.Settings.Default.P3DVersion = version;
                    }
                }
                Properties.Settings.Default.Save();
            }

        }

        public static void GetAllSettings()
        {
            try
            {
                FlightSim = FlightSimType.Unknown;
                GetAllFSPaths();
                var deleteOnClose = Properties.Settings.Default.DeleteImagesOnClose;
                if (deleteOnClose)
                {
                    deleteImages = AWDConvert.ToBoolean(deleteOnClose);
                }
                fsxAppVersion = Properties.Settings.Default.FSXVersion;
                fsxSEAppVersion = Properties.Settings.Default.FSXSEVersion;
                p3dAppVersion = Properties.Settings.Default.P3DVersion;
                FlightSim = GetFSType();
                PriorFlightSim = FlightSim;
                ChartService = Properties.Settings.Default.Service;
                ChartNotification = Properties.Settings.Default.ChartNotification;
            }
            catch (Exception ex)
            {
                logger.Error("Error getting Flight Sim settings. Error is: {0}", ex.Message);
            }
        }

        public static void GetAllFSPaths()
        {
            try
            {
                flightSimPaths = new Dictionary<FlightSimType, FSPaths>();

                if (File.Exists(appPath + @"\FSFlightBuilder.ini"))
                {
                    ConvertSettings();
                    CheckInstalls();
                }

                var objPath = new FSPaths
                {
                    AppDataPath = Properties.Settings.Default.FSXAppDataPath,
                    DefaultFSPath = Properties.Settings.Default.FSXDefaultFSPath,
                    CustomFSPath = Properties.Settings.Default.FSXCustomFSPath,
                    FPPath = Properties.Settings.Default.FSXFPPath,
                    AirplanesPath = Properties.Settings.Default.FSXAirplanesPath,
                    Installed = Properties.Settings.Default.FSXInstalled
                };

                flightSimPaths.Add(FlightSimType.FSX, objPath);

                objPath = new FSPaths
                {
                    AppDataPath = Properties.Settings.Default.FSXSEAppDataPath,
                    DefaultFSPath = Properties.Settings.Default.FSXSEDefaultFSPath,
                    CustomFSPath = Properties.Settings.Default.FSXSECustomFSPath,
                    FPPath = Properties.Settings.Default.FSXSEFPPath,
                    AirplanesPath = Properties.Settings.Default.FSXSEAirplanesPath,
                    Installed = Properties.Settings.Default.FSXSEInstalled
                };
                flightSimPaths.Add(FlightSimType.FSXSE, objPath);

                objPath = new FSPaths
                {
                    AppDataPath = Properties.Settings.Default.P3DAppDataPath,
                    DefaultFSPath = Properties.Settings.Default.P3DDefaultFSPath,
                    CustomFSPath = Properties.Settings.Default.P3DCustomFSPath,
                    FPPath = Properties.Settings.Default.P3DFPPath,
                    AirplanesPath = Properties.Settings.Default.P3DAirplanesPath,
                    Installed = Properties.Settings.Default.P3DInstalled
                };
                flightSimPaths.Add(FlightSimType.P3D, objPath);

                objPath = new FSPaths
                {
                    AppDataPath = Properties.Settings.Default.MSFSAppDataPath,
                    DefaultFSPath = Properties.Settings.Default.MSFSDefaultFSPath,
                    CustomFSPath = Properties.Settings.Default.MSFSCustomFSPath,
                    FPPath = Properties.Settings.Default.MSFSFPPath,
                    AirplanesPath = Properties.Settings.Default.MSFSAirplanesPath,
                    Installed = Properties.Settings.Default.MSFSInstalled
                };
                flightSimPaths.Add(FlightSimType.MSFS, objPath);
                pf3Path = Properties.Settings.Default.PF3Path;
            }
            catch (Exception ex)
            {
                logger.Error("Error getting Flight Sim paths. Error is: {0}", ex.Message);
            }
        }

        public static string CheckInstalls(bool secondTry = false)
        {
            GetAllFSPaths();

            var numFSFound = 0;
            if (!secondTry)
            {
                flightSimPaths[FlightSimType.FSX] = FSX.GetInstallPaths(flightSimPaths[FlightSimType.FSX]);
                if (!string.IsNullOrEmpty(flightSimPaths[FlightSimType.FSX].DefaultFSPath))
                {
                    numFSFound += 1;
                }
                flightSimPaths[FlightSimType.FSXSE] = FSXSE.GetInstallPaths(flightSimPaths[FlightSimType.FSXSE]);
                if (!string.IsNullOrEmpty(flightSimPaths[FlightSimType.FSXSE].DefaultFSPath))
                {
                    numFSFound += 1;
                }
                flightSimPaths[FlightSimType.P3D] = P3D.GetInstallPaths(flightSimPaths[FlightSimType.P3D]);
                if (!string.IsNullOrEmpty(flightSimPaths[FlightSimType.P3D].DefaultFSPath))
                {
                    numFSFound += 1;
                }
                flightSimPaths[FlightSimType.MSFS] = MSFS.GetInstallPaths(flightSimPaths[FlightSimType.MSFS]);
                if (!string.IsNullOrEmpty(flightSimPaths[FlightSimType.MSFS].DefaultFSPath))
                {
                    numFSFound += 1;
                }
            }

            if (numFSFound == 0)
            {
                if (!secondTry)
                {
                    MessageBox.Show(
                        @"Flight Simulator folders were not found.  Please select the flight sim folders.");
                    var frm = new FrmOptions();
                    frm.ShowDialog();
                    return CheckInstalls(true);
                }
                logger.Warn("No sim folders found. SecondTry = {0}", secondTry);
            }
            else
            {
                //Update the Settings
                Properties.Settings.Default.FSXAppDataPath = flightSimPaths[FlightSimType.FSX].AppDataPath;
                Properties.Settings.Default.FSXDefaultFSPath = flightSimPaths[FlightSimType.FSX].DefaultFSPath;
                Properties.Settings.Default.FSXCustomFSPath = flightSimPaths[FlightSimType.FSX].CustomFSPath;
                Properties.Settings.Default.FSXFPPath = flightSimPaths[FlightSimType.FSX].FPPath;
                Properties.Settings.Default.FSXAirplanesPath = flightSimPaths[FlightSimType.FSX].AirplanesPath;
                Properties.Settings.Default.FSXInstalled = flightSimPaths[FlightSimType.FSX].Installed;
                Properties.Settings.Default.FSXVersion = FSX.GetFSVersion(flightSimPaths[FlightSimType.FSX].DefaultFSPath);

                Properties.Settings.Default.FSXSEAppDataPath = flightSimPaths[FlightSimType.FSXSE].AppDataPath;
                Properties.Settings.Default.FSXSEDefaultFSPath = flightSimPaths[FlightSimType.FSXSE].DefaultFSPath;
                Properties.Settings.Default.FSXSECustomFSPath = flightSimPaths[FlightSimType.FSXSE].CustomFSPath;
                Properties.Settings.Default.FSXSEFPPath = flightSimPaths[FlightSimType.FSXSE].FPPath;
                Properties.Settings.Default.FSXSEAirplanesPath = flightSimPaths[FlightSimType.FSXSE].AirplanesPath;
                Properties.Settings.Default.FSXSEInstalled = flightSimPaths[FlightSimType.FSXSE].Installed;
                Properties.Settings.Default.FSXSEVersion = FSXSE.GetFSVersion(flightSimPaths[FlightSimType.FSXSE].DefaultFSPath);

                Properties.Settings.Default.P3DAppDataPath = flightSimPaths[FlightSimType.P3D].AppDataPath;
                Properties.Settings.Default.P3DDefaultFSPath = flightSimPaths[FlightSimType.P3D].DefaultFSPath;
                Properties.Settings.Default.P3DCustomFSPath = flightSimPaths[FlightSimType.P3D].CustomFSPath;
                Properties.Settings.Default.P3DFPPath = flightSimPaths[FlightSimType.P3D].FPPath;
                Properties.Settings.Default.P3DAirplanesPath = flightSimPaths[FlightSimType.P3D].AirplanesPath;
                Properties.Settings.Default.P3DInstalled = flightSimPaths[FlightSimType.P3D].Installed;
                Properties.Settings.Default.P3DVersion = P3D.GetFSVersion(flightSimPaths[FlightSimType.P3D].DefaultFSPath);

                Properties.Settings.Default.MSFSAppDataPath = flightSimPaths[FlightSimType.MSFS].AppDataPath;
                Properties.Settings.Default.MSFSDefaultFSPath = flightSimPaths[FlightSimType.MSFS].DefaultFSPath;
                Properties.Settings.Default.MSFSCustomFSPath = flightSimPaths[FlightSimType.MSFS].CustomFSPath;
                Properties.Settings.Default.MSFSFPPath = flightSimPaths[FlightSimType.MSFS].FPPath;
                Properties.Settings.Default.MSFSAirplanesPath = flightSimPaths[FlightSimType.MSFS].AirplanesPath;
                Properties.Settings.Default.MSFSInstalled = flightSimPaths[FlightSimType.MSFS].Installed;
                Properties.Settings.Default.MSFSVersion = MSFS.GetFSVersion(flightSimPaths[FlightSimType.MSFS].DefaultFSPath);

                Properties.Settings.Default.Save();

            }

            if (FlightSim != FlightSimType.Unknown && !string.IsNullOrEmpty(flightSimPaths[FlightSim].DefaultFSPath))
            {
                return flightSimPaths[FlightSim].DefaultFSPath;
            }
            else if (numFSFound > 1)
            {
                // Show the Popup Message
                var sims = new List<FlightSimType>();
                foreach (var sim in flightSimPaths)
                {
                    if (!string.IsNullOrEmpty(sim.Value.DefaultFSPath))
                    {
                        sims.Add(sim.Key);
                    }
                }

                var txt = "FS Flight Builder found multiple flight simulators." + Environment.NewLine + Environment.NewLine + "Please select the flight simulator you would like to load.";
                var popup = new PopupMessage("", txt, sims);
                if (popup.ShowDialog() == DialogResult.Yes)
                {
                    if (popup.SelectedFlightSim == FlightSimType.Unknown)
                    {
                        logger.Error("No flight simulator selected from the popup message.");
                        //Close the app
                        if (MessageBox.Show("FS Flight Builder cannot detect the installed flight simulator.", "FS Flight Builder Closing", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            Environment.Exit(-1);
                        }
                    }
                    var fsPath = flightSimPaths[popup.SelectedFlightSim].DefaultFSPath;
                    FlightSim = popup.SelectedFlightSim;
                    Properties.Settings.Default.FlightSim = FlightSim.ToString();
                    Properties.Settings.Default.Save();
                    //Make sure the selected flight sim paths are good.
                    CheckPaths();
                    logger.Info("Selected flight sim path is: {0}", fsPath);
                    return fsPath;
                }
                else
                {
                    //Close the app
                    if (MessageBox.Show("No flight sims found. FS Flight Builder cannot continue.", "FS Flight Builder Closing", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        Environment.Exit(-1);
                    }
                }
            }
            else if (numFSFound == 1)
            {
                foreach (var sim in flightSimPaths)
                {
                    if (!string.IsNullOrEmpty(sim.Value.DefaultFSPath))
                    {
                        return sim.Value.DefaultFSPath;
                    }
                }
            }

            return string.Empty;
        }

        public static string FormatFrequency(double freq)
        {
            const string fmt = "000.000";
            if (freq.ToString().Length == 5)
            {
                freq = freq / 100;
            }
            else
            {
                freq = freq / 1000;
            }
            return freq.ToString(fmt);
        }

        public static string FormatAltitude(decimal alt)
        {
            const string fmt = "000";
            alt = alt / 100;
            return alt.ToString(fmt);
        }

        public static string[] ParseParking(string parking)
        {
            //Get the parking text to name and number
            var idx = parking.IndexOf(" (");
            if (idx > -1)
            {
                parking = parking.Substring(0, idx);
            }

            string[] result = new string[2];

            //Find the last space
            idx = parking.LastIndexOf(' ');
            if (idx > -1)
            {
                result[0] = ParkingUtil.parkingNameToStrRev(parking.Substring(0, idx));
                result[1] = parking.Substring(idx + 1);
            }
            return result;
        }

        public static Seasons Season(DateTime date)
        {
            /* Astronomically Spring begins on March 21st, the 80th day of the year. 
             * Summer begins on the 172nd day, Autumn, the 266th and Winter the 355th.
             * Of course, on a leap year add one day to each, 81, 173, 267 and 356.*/

            int doy = date.DayOfYear - AWDConvert.ToInt32((DateTime.IsLeapYear(date.Year)) && date.DayOfYear > 59);

            if (doy < 80 || doy >= 355) return Seasons.Winter;

            if (doy >= 80 && doy < 172) return Seasons.Spring;

            if (doy >= 172 && doy < 266) return Seasons.Summer;

            return Seasons.Fall;
        }

        public static bool CheckPaths()
        {
            //If we don't have the selected sim or the paths are missing, re-run the checkinstalls method
            if (FlightSim == FlightSimType.Unknown || string.IsNullOrEmpty(flightSimPaths[FlightSim].DefaultFSPath) || string.IsNullOrEmpty(flightSimPaths[FlightSim].FPPath))
            {
                CheckInstalls();
            }

            string path = string.Empty;
            switch (FlightSim)
            {
                case FlightSimType.FSX:
                    path = GetAppPath(@"Microsoft\FSX\", "FSX.CFG");
                    break;
                case FlightSimType.FSXSE:
                    path = GetAppPath(@"Microsoft\FSX-SE\", "FSX_SE.CFG");
                    if (!File.Exists(path))
                    {
                        path = GetAppPath(@"Microsoft\FSX\", "FSX.CFG");
                    }
                    break;
                case FlightSimType.P3D:
                    path = GetAppPath(@"Lockheed Martin\Prepar3D v5\", "prepar3d.cfg");
                    break;
                case FlightSimType.MSFS:
                    path = GetAppPath(@"Microsoft Flight Simulator\", "UserCfg.opt");
                    break;
            }

            if (FlightSim == FlightSimType.Unknown || string.IsNullOrEmpty(path))
            {
                MessageBox.Show(@"FS Flight Builder cannot determine your installed Flight Sim.  Please re-launch the application and try again." +
                    Environment.NewLine + Environment.NewLine + "If you believe this is an error, please contact the developer at tarduini@arduiniwebdevelopment.com",
                     @"Cannot Determine Flight Sim", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (File.Exists(appPath + @"\FSFlightBuilder.ini"))
                {
                    File.Delete(appPath + @"\FSFlightBuilder.ini");
                }
                Environment.Exit(-1);
            }
            return true;
        }

        public static string GetAppPath(string path, string file)
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //Find the config file and change the setting to not show the Initial Dialog
            var appPath = string.Empty;
            if (!string.IsNullOrEmpty(appDataFolder))
            {
                appPath = appDataFolder + @"\" + path;
                if (FlightSim == FlightSimType.P3D)
                {
                    if (!Directory.Exists(appPath))
                    {
                        appPath = appDataFolder + @"\Prepar3D v4\";
                        if (!Directory.Exists(appPath))
                        {
                            appPath = appDataFolder + @"\Prepar3D v3\";
                            if (!Directory.Exists(appPath))
                            {
                                appPath = appDataFolder + @"\Prepar3D v2\";
                                if (!Directory.Exists(appPath))
                                {
                                    appPath = appDataFolder + @"\Prepar3D\";
                                }
                            }
                        }
                    }
                }

                //Find the config file
                if (File.Exists(appPath + file) && File.Exists(appPath + "AWD.cfg"))
                {
                    //Delete the config file
                    File.Delete(appPath + file);
                    //Rename the AWD.config to config
                    File.Move(appPath + "AWD.cfg", appPath + file);
                }
            }
            return appPath;
        }

        public static FlightInfo ParseFlightFile(string fileName, List<Aircraft> aircraft)
        {
            var fName = fileName.ToLower();
            var info = new FlightInfo();
            if (fName.EndsWith(".flt") || fName.EndsWith(".fxml"))
            {
                info.FlightFile = fileName;
            }
            else if (fName.EndsWith(".pln"))
            {
                info.FlightPlanFile = fileName;
            }
            var f = new FileInfo(fileName);
            var path = f.DirectoryName;
            if (!string.IsNullOrEmpty(info.FlightFile) && File.Exists(info.FlightFile))
            {
                if (fName.EndsWith(".fxml"))
                {
                    string xml;
                    using (StreamReader sr = new StreamReader(info.FlightFile))
                    {
                        xml = sr.ReadToEnd();
                    }
                    xml = xml.Replace("& ", "&amp;");
                    var flightFile = info.FlightFile.Replace(".fxml", "_awd.fxml");
                    var htmWriter = new StreamWriter(flightFile);
                    htmWriter.Write(xml);
                    htmWriter.Close();
                    try
                    {
                        var xmlDoc = XDocument.Load(flightFile);
                        var items = from item in xmlDoc.Elements("SimBase.Document").Elements("Flight.Sections")
                                    select item;
                        foreach (var item in items)
                        {
                            foreach (var section in item.Elements())
                            {
                                switch (section.Attribute("Name").Value)
                                {
                                    case "GPS_Engine":
                                        foreach (var elem in section.Elements())
                                        {
                                            if (elem.HasAttributes)
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "Filename":
                                                        info.FlightPlanFile = path + @"\" +
                                                                              elem.Attribute("Value").Value +
                                                                              ".pln";
                                                        break;
                                                    case "WpInfo0":
                                                        var parts = elem.Attribute("Value").Value.Split(',');
                                                        info.CruiseSpeed = parts[0].TrimStart('0');
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case "DateTimeSeason":
                                        int year = DateTime.Now.Year;
                                        int day = DateTime.Now.DayOfYear;
                                        int hour = DateTime.Now.Hour;
                                        int min = DateTime.Now.Minute;
                                        int sec = DateTime.Now.Second;

                                        foreach (var elem in section.Elements())
                                        {
                                            if (elem.HasAttributes)
                                            {
                                                switch (elem.Attribute("Name")?.Value)
                                                {
                                                    case "Year":
                                                        year = AWDConvert.ToInt32(elem.Attribute("Value")?.Value);
                                                        break;
                                                    case "Day":
                                                        day = AWDConvert.ToInt32(elem.Attribute("Value")?.Value);
                                                        break;
                                                    case "Hours":
                                                        hour = AWDConvert.ToInt32(elem.Attribute("Value")?.Value);
                                                        break;
                                                    case "Minutes":
                                                        min = AWDConvert.ToInt32(elem.Attribute("Value")?.Value);
                                                        break;
                                                    case "Seconds":
                                                        sec = AWDConvert.ToInt32(elem.Attribute("Value")?.Value);
                                                        break;
                                                }
                                            }
                                        }
                                        info.FlightTime =
                                            new DateTime(year, 1, 1).AddDays(day - 1)
                                                .AddHours(hour)
                                                .AddMinutes(min)
                                                .AddSeconds(sec);
                                        break;
                                    case "Sim.0":
                                        foreach (var elem in section.Elements())
                                        {
                                            if (elem.HasAttributes)
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "Sim":
                                                        //Get the aircraft text
                                                        var acft =
                                                            aircraft.FirstOrDefault(
                                                                a =>
                                                                {
                                                                    var xAttribute = elem.Attribute("Value");
                                                                    return
                                                                        xAttribute != null && a.Name.Contains(
                                                                            xAttribute.Value);
                                                                });
                                                        if (acft != null)
                                                        {
                                                            info.Aircraft = acft;
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case "Weather":
                                        foreach (var elem in section.Elements())
                                        {
                                            if (elem.HasAttributes)
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "WeatherType":
                                                        info.WeatherType = elem.Attribute("Value")?.Value;
                                                        break;
                                                    case "ThemeName":
                                                        info.WeatherTheme = elem.Attribute("Value")?.Value;
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case "ObjectFile":
                                        foreach (var elem in section.Elements())
                                        {
                                            if (elem.HasAttributes)
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "File":
                                                        info.XmlFile = path + @"\" + elem.Attribute("Value").Value +
                                                                       ".xml";
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(
                            @"Could not read the file """ + fileName +
                            @""".  Please make sure it is formatted correctly.", @"Error Reading File",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (!string.IsNullOrEmpty(flightFile) && flightFile.EndsWith("_awd.fxml"))
                        {
                            File.Delete(flightFile);
                        }
                        throw new Exception("An error occurred in ParseFlightFile.", e);
                    }
                    File.Delete(flightFile);
                }
                else if (fName.EndsWith(".flt"))
                {
                    int year = DateTime.Now.Year;
                    int day = DateTime.Now.DayOfYear;
                    int hour = DateTime.Now.Hour;
                    int min = DateTime.Now.Minute;
                    var reader = new StreamReader(File.OpenRead(fileName));
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            var parts = line.Split('=');
                            if (parts.Length > 1)
                            {
                                switch (parts[0])
                                {
                                    case "Filename":
                                        info.FlightPlanFile = parts[1] + ".pln";
                                        break;
                                    case "WpInfo0":
                                        foreach (var val in parts)
                                        {
                                            if (!val.Contains("WpInfo0"))
                                            {
                                                var wpParts = val.Split(',');
                                                info.CruiseSpeed = wpParts[0].TrimStart('0');
                                                break;
                                            }
                                        }
                                        break;
                                    case "routetype":
                                        switch (parts[1])
                                        {
                                            case "1":
                                                info.RouteType = "VOR to VOR";
                                                break;
                                            case "2":
                                                info.RouteType = "Low Altitude Airways";
                                                break;
                                            case "3":
                                                info.RouteType = "High Altitude Airways";
                                                break;
                                            default:
                                                info.RouteType = "Direct";
                                                break;
                                        }
                                        break;
                                    case "cruising_altitude":
                                        info.CruiseAltitude = parts[1];
                                        break;
                                    case "Year":
                                        year = AWDConvert.ToInt32(parts[1]);
                                        break;
                                    case "Day":
                                        day = AWDConvert.ToInt32(parts[1]);
                                        break;
                                    case "Hours":
                                        hour = AWDConvert.ToInt32(parts[1]);
                                        break;
                                    case "Minutes":
                                        min = AWDConvert.ToInt32(parts[1]);
                                        break;
                                    case "Seconds":
                                        int sec = AWDConvert.ToInt32(parts[1]);
                                        info.FlightTime =
                                            new DateTime(year, 1, 1).AddDays(day - 1)
                                                .AddHours(hour)
                                                .AddMinutes(min)
                                                .AddSeconds(sec);
                                        break;
                                    case "WeatherType":
                                        info.WeatherType = parts[1];
                                        break;
                                    case "ThemeName":
                                        info.WeatherTheme = parts[1];
                                        break;
                                    case "Sim":
                                        //Get the aircraft text
                                        var acft = aircraft.FirstOrDefault(a => a.Name.Contains(parts[1]));
                                        if (acft != null)
                                        {
                                            info.Aircraft = acft;
                                        }
                                        break;
                                    case "File":
                                        info.XmlFile = path + @"\" + parts[1] + ".xml";
                                        break;
                                }
                            }
                        }
                    }
                    reader.Close();

                    if (string.IsNullOrEmpty(info.FlightPlanFile))
                    {
                        info.FlightPlanFile = fName.Replace(".flt", ".pln");
                    }
                    else if (!File.Exists(info.FlightPlanFile))
                    {
                        info.FlightPlanFile = fName.Replace(".flt", ".pln");
                    }
                }
            }

            //Flight Plan
            if (!string.IsNullOrEmpty(info.FlightPlanFile) && File.Exists(info.FlightPlanFile) && new FileInfo(info.FlightPlanFile).Length > 0)
            {
                //Open the pln file to get waypoints and parking
                var xmlDoc = XDocument.Load(info.FlightPlanFile);
                var waypoints = new List<string>();
                var items = from item in xmlDoc.Elements("SimBase.Document").Elements("FlightPlan.FlightPlan")
                            select item;
                foreach (var item in items)
                {
                    string icao = null;
                    foreach (var element in item.Elements())
                    {
                        switch (element.Name.ToString())
                        {
                            case "DepartureID":
                                icao = element.Value;
                                break;
                            case "DeparturePosition":
                                var park = string.Empty;
                                var n = string.Empty;
                                if (!string.IsNullOrEmpty(icao))
                                {
                                    info.Parking = element.Value;
                                    var parts = element.Value.Split(' ');
                                    if (parts.Length > 1)
                                    {
                                        if (parts[0].Contains("GATE"))
                                        {
                                            if (parts.Length > 2)
                                            {
                                                park = parts[1];
                                                n = parts[2];
                                            }
                                            else
                                            {
                                                park = parts[1].Substring(0, 1);
                                                n = parts[1].Substring(1);
                                            }
                                        }
                                        else
                                        {
                                            park = "PARKING";
                                            n = parts[1];
                                        }
                                    }
                                    else if (parts.Length == 1)
                                    {
                                        info.Runway = AWDConvert.ToInt32(parts[0]);
                                        break;
                                    }
                                }
                                break;
                            case "ATCWaypoint":
                                waypoints.Add(element.Attribute("id").Value);
                                break;
                            case "RouteType":
                                if (string.IsNullOrEmpty(info.RouteType))
                                {
                                    switch (element.Value)
                                    {
                                        case "VOR":
                                            info.RouteType = "VOR to VOR";
                                            break;
                                        case "LowAlt":
                                            info.RouteType = "Low Altitude Airways";
                                            break;
                                        case "HighAlt":
                                            info.RouteType = "High Altitude Airways";
                                            break;
                                        default:
                                            info.RouteType = "Direct";
                                            break;
                                    }
                                }
                                break;
                            case "FPType":
                                info.FpType = element.Value;
                                break;
                            case "CruisingAlt":
                                if (string.IsNullOrEmpty(info.CruiseAltitude))
                                {
                                    info.CruiseAltitude = element.Value;
                                }
                                break;
                        }


                    }
                    break;
                }
                info.Route = waypoints;
            }


            //Open the xml file to get the briefing.
            if (!string.IsNullOrEmpty(info.XmlFile) && File.Exists(info.XmlFile))
            {
                var xmlDoc = XDocument.Load(info.XmlFile);
                var items = from item in xmlDoc.Elements("SimBase.Document").Elements("WorldBase.Flight").Elements("SimMissionUI.ScenarioMetadata")
                            select item;
                foreach (var item in items)
                {
                    foreach (var element in item.Elements())
                    {
                        switch (element.Name.ToString())
                        {
                            case "AbbreviatedMissionBrief":
                                info.BriefingFile = path + @"\" + element.Value;
                                break;
                        }
                    }
                    break;
                }
            }

            return info;
        }

        public static void SendMail(string message)
        {
            try
            {
                var mail = new MailMessage();
                var SmtpServer = new SmtpClient("mail.arduiniwebdevelopment.com");

                mail.From = new MailAddress("tarduini@arduiniwebdevelopment.com");
                mail.To.Add("tarduini@arduiniwebdevelopment.com");
                mail.Subject = "FS Flight Builder Error";
                mail.Body = "An error has occurred in the application.  The error is:" + message;

                SmtpServer.Port = 25; //465; //25; //587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("tarduini@arduiniwebdevelopment.com", "birdman8728J!");

                SmtpServer.Send(mail);
            }
            catch
            {
                // ignored
            }
        }

        public static List<RoutePoint> GetRoutePoints(List<string> route, Airport dep, Airport dest,
            Aircraft acft, decimal cruise, bool includeTOC, bool includeTOD)
        {
            var routePoints = new List<RoutePoint>
            {
                new RoutePoint
                {
                    Id = dep.AirportId,
                        Region = string.Empty,
                    Latitude = dep.Latitude,
                    Longitude = dep.Longitude,
                    Elevation = (int)dep.Elevation,
                    MagVar = dep.MagVar,
                    Type = "Airport"
                }
            };

            using (var context = new FSFBDbConn(DBConnName, DataPath))
            {
                dep.MagVar = dep.MagVar * -1;
                dest.MagVar = dest.MagVar * -1;
                foreach (var rte in route)
                {
                    var nav = context.Navaids.FirstOrDefault(n => n.NavId == rte && n.FSType == (int)FlightSim);
                    if (nav != null)
                    {
                        routePoints.Add(new RoutePoint
                        {
                            Id = nav.NavId,
                            Region = nav.Region,
                            Latitude = nav.Latitude,
                            Longitude = nav.Longitude,
                            Elevation = 0,
                            MagVar = (double)nav.MagVar,
                            Type = nav.Type,
                            Frequency = nav.Frequency
                        });
                    }
                    else
                    {
                        var wpt = context.Waypoints.Include("Routes").FirstOrDefault(n => n.NavId == rte && n.FSType == (int)FlightSim);
                        if (wpt != null)
                        {
                            routePoints.Add(new RoutePoint
                            {
                                Id = wpt.NavId,
                                Region = wpt.Region,
                                Latitude = wpt.Latitude,
                                Longitude = wpt.Longitude,
                                Elevation = 0,
                                MagVar = wpt.MagVar,
                                Type = wpt.Type
                            });
                        }
                        else if (rte != dep.AirportId && rte != dest.AirportId)
                        {
                            var apt = context.Airports.Include("Runways").Include("Parkings").Include("Comms").FirstOrDefault(a => a.AirportId == rte && a.FSType == (int)FlightSim);
                            if (apt != null)
                            {
                                routePoints.Add(new RoutePoint
                                {
                                    Id = apt.AirportId,
                                    Region = string.Empty,
                                    Latitude = apt.Latitude,
                                    Longitude = apt.Longitude,
                                    Elevation = (long)apt.Elevation,
                                    MagVar = apt.MagVar,
                                    Type = "Airport",
                                    Frequency = string.Empty
                                });
                            }
                            else
                            {
                                MessageBox.Show(rte + " was not found in the flight simulator database.", "Waypoint Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                routePoints.Add(new RoutePoint
                {
                    Id = dest.AirportId,
                    Region = string.Empty,
                    Latitude = dest.Latitude,
                    Longitude = dest.Longitude,
                    Elevation = (long)dest.Elevation,
                    MagVar = dest.MagVar,
                    Type = "Airport"
                });

                //Now I should have a list of all waypoints
                if (includeTOC)
                {
                    var departure = new GeoLocation
                    {
                        Latitude = routePoints[0].Latitude,
                        Longitude = routePoints[0].Longitude
                    };
                    var wpNext = new GeoLocation
                    {
                        Latitude = routePoints[1].Latitude,
                        Longitude = routePoints[1].Longitude
                    };
                    var from = departure;

                    var tc = DegreeBearing(departure, wpNext);
                    var th = tc + WCA(tc, Convert.ToDouble(acft.ClimbSpeed),
                                 Convert.ToDouble(dep.WindDirection), Convert.ToDouble(dep.WindSpeed));
                    var heading = th + Convert.ToDouble(dep.MagVar);
                    var gsInKnots = Groundspeed(heading, Convert.ToDouble(acft.ClimbSpeed),
                        Convert.ToDouble(dep.WindDirection), Convert.ToDouble(dep.WindSpeed));
                    //Time for Climb [(Cruise Alt-Dep Alt)/Climb Rate] = 8.5 minutes
                    var timeInMinutes = Convert.ToDouble((cruise - dep.Elevation) / acft.ClimbRate);
                    var distanceInKilometers = Distance(timeInMinutes, gsInKnots);

                    //Determine distance between departure and WP1
                    var wpDistanceInKilometers = Distance(departure, wpNext);

                    //if TOC distance is greater than distance above then 
                    var i = 1;
                    while (distanceInKilometers > wpDistanceInKilometers && routePoints.Count > i)
                    {
                        i++;
                        from = wpNext;
                        wpNext = new GeoLocation
                        {
                            Latitude = routePoints[i].Latitude,
                            Longitude = routePoints[i].Longitude
                        };
                        //   Calculate climb elevation to WP1   
                        timeInMinutes = wpDistanceInKilometers / gsInKnots * 60;
                        var wpElevation = timeInMinutes * Convert.ToDouble(acft.ClimbRate);
                        //   new distance to climb will be WP1 cruise altitude - WP1 altitude
                        timeInMinutes = Convert.ToDouble((cruise - Convert.ToDecimal(wpElevation)) / acft.ClimbRate);
                        tc = DegreeBearing(from, wpNext);
                        th = tc + WCA(tc, Convert.ToDouble(acft.ClimbSpeed),
                                     Convert.ToDouble(dep.WindDirection), Convert.ToDouble(dep.WindSpeed));
                        heading = th + Convert.ToDouble(routePoints[i - 1].MagVar);
                        gsInKnots = Groundspeed(heading, Convert.ToDouble(acft.ClimbSpeed),
                            Convert.ToDouble(dep.WindDirection), Convert.ToDouble(dep.WindSpeed));
                        distanceInKilometers = Distance(timeInMinutes, gsInKnots);
                        wpDistanceInKilometers = Distance(from, wpNext);
                    }

                    if (wpDistanceInKilometers > distanceInKilometers)
                    {
                        var TOC = FindPointAtDistanceFrom(from, tc, distanceInKilometers);
                        var point = new RoutePoint
                        {
                            Id = "TOC",
                            Latitude = TOC.Latitude,
                            Longitude = TOC.Longitude,
                            MagVar = routePoints[i - 1].MagVar,
                            Type = "VOR"
                        };
                        routePoints.Insert(i, point);
                    }
                }

                //TOD
                if (includeTOD)
                {
                    bool calcTOD = true;
                    var destination = new GeoLocation
                    {
                        Latitude = routePoints[routePoints.Count - 1].Latitude,
                        Longitude = routePoints[routePoints.Count - 1].Longitude
                    };

                    if (destination.Latitude > 90 || destination.Latitude < -90)
                    {
                        MessageBox.Show($"Cannot calculate TOD.  The latitude for {routePoints[routePoints.Count - 1].Id} must be between -90 and 90.  It is currently set to {destination.Latitude}");
                        calcTOD = false;
                    }
                    if (destination.Longitude > 180 || destination.Longitude < -180)
                    {
                        MessageBox.Show($"Cannot calculate TOD.  The longitude for {routePoints[routePoints.Count - 1].Id} must be between -180 and 180.  It is currently set to {destination.Longitude}");
                        calcTOD = false;
                    }


                    var wpPrevious = new GeoLocation
                    {
                        Latitude = routePoints[routePoints.Count - 2].Latitude,
                        Longitude = routePoints[routePoints.Count - 2].Longitude
                    };
                    if (wpPrevious.Latitude > 90 || wpPrevious.Latitude < -90)
                    {
                        MessageBox.Show($"Cannot calculate TOD.  The latitude for {routePoints[routePoints.Count - 2].Id} must be between -90 and 90.  It is currently set to {wpPrevious.Latitude}");
                        calcTOD = false;
                    }
                    if (wpPrevious.Longitude > 180 || wpPrevious.Longitude < -180)
                    {
                        MessageBox.Show($"Cannot calculate TOD.  The longitude for {routePoints[routePoints.Count - 2].Id} must be between -180 and 180.  It is currently set to {wpPrevious.Longitude}");
                        calcTOD = false;
                    }

                    if (calcTOD)
                    {

                        var tc = DegreeBearing(wpPrevious, destination);
                        var th = tc + WCA(tc, Convert.ToDouble(acft.DescentSpeed),
                                     Convert.ToDouble(dest.WindDirection), Convert.ToDouble(dest.WindSpeed));
                        var heading = th + Convert.ToDouble(dest.MagVar);
                        var gsInKnots = Groundspeed(heading, Convert.ToDouble(acft.DescentSpeed),
                            Convert.ToDouble(dest.WindDirection), Convert.ToDouble(dest.WindSpeed));
                        var aptElev = cruise - (dest.Elevation + 800);
                        var distanceInKilometers = TODDistance(Convert.ToDouble(aptElev), Convert.ToDouble(acft.DescentRate), gsInKnots);

                        //Determine distance between last waypoint and destination
                        var wpDistanceInKilometers = Distance(wpPrevious, destination);

                        //if TOD distance is greater than distance above then
                        var i = 2;
                        while (distanceInKilometers > wpDistanceInKilometers && routePoints.Count > i)
                        {
                            i++;
                            var from = wpPrevious;
                            wpPrevious = new GeoLocation
                            {
                                Latitude = routePoints[routePoints.Count - i].Latitude,
                                Longitude = routePoints[routePoints.Count - i].Longitude
                            };
                            //   Calculate climb elevation to WP1   
                            var time = wpDistanceInKilometers / gsInKnots * 60;
                            var wpElevation = time * Convert.ToDouble(acft.DescentRate);
                            //   new distance to climb will be WP1 cruise altitude - WP1 altitude
                            time = Convert.ToDouble((cruise - Convert.ToDecimal(wpElevation)) / acft.DescentRate);
                            tc = DegreeBearing(wpPrevious, from);
                            th = tc + WCA(tc, Convert.ToDouble(acft.DescentRate),
                                         Convert.ToDouble(dest.WindDirection), Convert.ToDouble(dest.WindSpeed));
                            heading = th + Convert.ToDouble(routePoints[routePoints.Count - i + 1].MagVar);
                            gsInKnots = Groundspeed(heading, Convert.ToDouble(acft.DescentSpeed),
                                Convert.ToDouble(dest.WindDirection), Convert.ToDouble(dest.WindSpeed));
                            distanceInKilometers = Distance(time, gsInKnots);
                            wpDistanceInKilometers = Distance(wpPrevious, from);
                        }

                        if (wpDistanceInKilometers > distanceInKilometers)
                        {
                            var TOD = FindPointAtDistanceFrom(wpPrevious, tc, wpDistanceInKilometers - distanceInKilometers);
                            var point = new RoutePoint
                            {
                                Id = "TOD",
                                Latitude = TOD.Latitude,
                                Longitude = TOD.Longitude,
                                MagVar = routePoints[routePoints.Count - i + 1].MagVar,
                                Type = "Waypoint"
                            };
                            routePoints.Insert(routePoints.Count - i + 1, point);
                        }
                    }
                }
            }

            return routePoints;
        }

        public static string UpdateWeather(string icao)
        {
            var dict = new eDictionary();
            var txtRoute = new RichTextBox();
            return UpdateWeather(icao, string.Empty, null, null, ref dict, ref txtRoute);
        }

        public static string CheckAirac(bool ignoreDownload = false)
        {
            // Check to see if the AIRAC version is in the settings
            var downloadAirac = false;
            var airac = Properties.Settings.Default.AIRAC;
            downloadAirac = string.IsNullOrEmpty(airac) || airac.Length != 4 || !File.Exists(DataPath + @"\d-tpp_Metafile.xml");

            if (!downloadAirac)
            {
                // If it is, see if it matches the current year and month.  If not, do same as above
                var year = 2000 + AWDConvert.ToInt32(airac.Substring(0, 2));
                var month = AWDConvert.ToInt32(airac.Substring(2, 2));
                downloadAirac = year < DateTime.Now.Year || month < DateTime.Now.Month;
            }

            if (!ignoreDownload && downloadAirac)
            {
                if (!File.Exists(DataPath + @"\d-tpp_Metafile.xml"))
                {
                    // Must download
                    MessageBox.Show(@"Downloading the chart data.  This may take a moment.", "Chart Data Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //Optional download
                    if (MessageBox.Show(@"There is an update to the chart data.  Would you like to download it?", "Chart Data Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                    {
                        downloadAirac = false;
                    }
                }
            }

            if (!ignoreDownload && downloadAirac)
            {
                // if it's not, generate the AIRAC number and download the new d-tpp-Metafile.xml
                var year = DateTime.Now.Year.ToString().Substring(2, 2);
                var month = DateTime.Now.Month.ToString();
                if (month.Length < 2)
                {
                    month = "0" + month;
                }

                airac = year + month;
                using (var client = new HttpClient())
                {
                    var url = $"http://aeronav.faa.gov/d-tpp/{airac}/xml_data/d-tpp_Metafile.xml";
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var responseContent = response.Content;
                            var airacResult = responseContent.ReadAsStringAsync().Result;
                            var filePath = $"{DataPath}\\d-tpp_Metafile.xml";
                            File.WriteAllText(filePath, airacResult);

                            //client..DownloadFile(url, DataPath + @"\d-tpp_Metafile.xml");
                            Properties.Settings.Default.AIRAC = airac;
                            Properties.Settings.Default.Save();
                        }
                        catch (ArgumentNullException)
                        {
                            // Change the airac cycle back one and try again
                            year = DateTime.Now.AddMonths(-1).Year.ToString().Substring(2, 2);
                            month = DateTime.Now.AddMonths(-1).Month.ToString();
                            if (month.Length < 2)
                            {
                                month = "0" + month;
                            }

                            airac = year + month;

                            url = $"http://aeronav.faa.gov/d-tpp/{airac}/xml_data/d-tpp_Metafile.xml";
                            response = client.GetAsync(url).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                try
                                {
                                    var responseContent = response.Content;
                                    var airacResult = responseContent.ReadAsStringAsync().Result;
                                    var filePath = $"{DataPath}\\d-tpp_Metafile.xml";
                                    File.WriteAllText(filePath, airacResult);
                                    Properties.Settings.Default.AIRAC = airac;
                                    Properties.Settings.Default.Save();
                                }
                                catch
                                {
                                    MessageBox.Show("Unable to connect to the chart service.  Please check your internet connection and try again.");
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unable to connect to the chart service.  Please check your internet connection and try again.");
                        }
                    }
                }
            }
            return airac;
        }

        public static string UpdateWeather(string dep, string dest, FSFlightBuilder.Data.Models.Airport _departure, FSFlightBuilder.Data.Models.Airport _destination, ref eDictionary dict, ref RichTextBox txtRoute)
        {
            var returnVal = new StringBuilder();
            //Weather
            try
            {
                using (var client = new HttpClient())
                {
                    var url = string.IsNullOrEmpty(dest)
                        ? $"http://www.aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&requestType=retrieve&format=xml&hoursBeforeNow=12&stationString={dep}"
                        : $"http://www.aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&requestType=retrieve&format=xml&hoursBeforeNow=12&stationString={dep},{dest}";
                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var responseContent = response.Content;
                            var weatherResult = responseContent.ReadAsStringAsync().Result;
                            //            var weatherResult = 


                            //                                        WebClient wc = new WebClient();
                            //Uri uri;
                            //if (string.IsNullOrEmpty(dest))
                            //{
                            //    uri = new Uri(
                            //        "http://www.aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&requestType=retrieve&format=xml&hoursBeforeNow=12&stationString=" +
                            //        dep);
                            //}
                            //else
                            //{
                            //    uri = new Uri(
                            //        "http://www.aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&requestType=retrieve&format=xml&hoursBeforeNow=12&stationString=" +
                            //        dep + "," + dest);
                            //}
                            //var weatherResult = wc.DownloadString(uri);

                            weatherResult = weatherResult.Replace("\r\n", ""); //.Replace("\"",@"|").Replace("|",@"""");
                            var xmlDoc = XDocument.Parse(weatherResult);
                            var items =
                                from item in
                                xmlDoc.Elements("response")
                                    .Elements("data")
                                    .Elements("METAR")
                                select item;
                            var destdone = string.IsNullOrEmpty(dest);
                            var depdone = false;
                            //                _isIFR = false;
                            dict = new eDictionary();
                            foreach (var item in items)
                            {
                                var xElement = item.Element("station_id");
                                if (xElement != null)
                                {
                                    var icao = xElement.Value;
                                    string cat = string.Empty;
                                    int vis = 0;
                                    if ((!depdone && icao == dep) || (!destdone && !string.IsNullOrEmpty(dest) && icao == dest))
                                    {
                                        var weather = new StringBuilder();
                                        var fc = item.Element("flight_category");
                                        if (fc != null)
                                        {
                                            cat = fc.Value;
                                        }
                                        else
                                        {
                                            var vs = item.Element("visibility_statute_mi");
                                            if (vs != null)
                                            {
                                                vis = AWDConvert.ToInt32(vs.Value);
                                            }
                                            var sc = item.Element("sky_condition");
                                            if (sc != null)
                                            {
                                                var cb = sc.Attribute("cloud_base_ft_agl");
                                                var sCover = sc.Attribute("sky_cover");
                                                if (cb != null && sCover != null)
                                                {
                                                    var clouds = AWDConvert.ToInt32(cb.Value);

                                                    if (sCover.Value == "OVC" || sCover.Value == "BKN")
                                                    {
                                                        if (vis > 0 && vis < 1 || clouds > 0 && clouds <= 500)
                                                        {
                                                            cat = "LIFR";
                                                        }
                                                        else if (vis > 0 && vis <= 3 || clouds > 0 && clouds <= 1000)
                                                        {
                                                            cat = "IFR";
                                                        }
                                                        else if (vis > 0 && vis <= 5 || clouds > 0 && clouds <= 3000)
                                                        {
                                                            cat = "MVFR";
                                                        }
                                                        else if (vis > 0 && clouds > 0)
                                                        {
                                                            cat = "VFR";
                                                        }
                                                        else
                                                        {
                                                            cat = "Unknown";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (vis > 0 && vis <= 5 || clouds > 0 && clouds <= 3000)
                                                        {
                                                            cat = "MVFR";
                                                        }
                                                        else if (vis > 0 && clouds > 0)
                                                        {
                                                            cat = "VFR";
                                                        }
                                                        else
                                                        {
                                                            cat = "Unknown";
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        weather.Append("Flight Category: " + cat + Environment.NewLine);

                                        var alt = item.Element("altim_in_hg");
                                        if (alt != null)
                                        {
                                            weather.Append("Pressure: " +
                                                           AWDConvert.ToDecimal(alt.Value)
                                                               .ToString("N2") +
                                                           " in.");
                                            var sl = item.Element("sea_level_pressure_mb");
                                            if (sl != null)
                                            {
                                                weather.Append("(" +
                                                               AWDConvert.ToDecimal(
                                                                       sl.Value)
                                                                   .ToString("N1") +
                                                               " mb.)");
                                            }
                                            weather.Append(Environment.NewLine);
                                        }
                                        else
                                        {
                                            weather.Append("Pressure: Standard" + Environment.NewLine);
                                        }

                                        var ot = item.Element("observation_time");
                                        if (ot != null)
                                        {
                                            weather.Append("Observation Time: " + ot.Value +
                                                           Environment.NewLine);
                                        }
                                        var temp = item.Element("temp_c");
                                        if (temp != null)
                                        {
                                            var val = AWDConvert.ToDecimal(temp.Value) * 9 / 5 + 32;
                                            weather.Append("Temperature: " + temp.Value +
                                                           "&deg; celcius (" +
                                                           val +
                                                           "&deg; fahrenheit)" + Environment.NewLine);
                                        }
                                        var dp = item.Element("dewpoint_c");
                                        if (dp != null)
                                        {
                                            var val = AWDConvert.ToDecimal(dp.Value) * 9 / 5 + 32;
                                            weather.Append("Dewpoint: " + dp.Value +
                                                           "&deg; celcius (" +
                                                           val +
                                                           "&deg; fahrenheit)" + Environment.NewLine);
                                        }
                                        var wind = item.Element("wind_speed_kt");
                                        var speed = -1m;
                                        var dir = -1m;
                                        if (wind != null)
                                        {
                                            speed = AWDConvert.ToDecimal(wind.Value);
                                            if (icao == dep && _departure != null)
                                            {
                                                _departure.WindSpeed = (long)speed;
                                            }
                                            else if (icao == dest && _destination != null)
                                            {
                                                _destination.WindSpeed = (long)speed;
                                            }
                                        }
                                        var elemDir = item.Element("wind_dir_degrees");
                                        if (elemDir != null)
                                        {
                                            dir = AWDConvert.ToDecimal(elemDir.Value);
                                            if (icao == dep && _departure != null)
                                            {
                                                _departure.WindDirection = (long)dir;
                                            }
                                            else if (icao == dest && _destination != null)
                                            {
                                                _destination.WindDirection = (long)dir;
                                            }
                                        }
                                        if (wind != null && speed > 0 && elemDir != null)
                                        {
                                            weather.Append("Wind: " + dir +
                                                           "&deg; @ " +
                                                           item.Element("wind_speed_kt").Value + " kts" +
                                                           Environment.NewLine);
                                        }
                                        else if (speed == 0)
                                        {
                                            weather.Append("Wind: Calm" + Environment.NewLine);
                                        }
                                        else
                                        {
                                            weather.Append("Wind: Unavailable" + Environment.NewLine);
                                        }
                                        if (item.Element("visibility_statute_mi") != null)
                                        {
                                            weather.Append("Visibility (sm): " +
                                                           item.Element("visibility_statute_mi").Value +
                                                           Environment.NewLine);
                                        }
                                        if (item.Element("altim_in_hg") != null)
                                        {
                                            weather.Append("Pressure: " +
                                                           AWDConvert.ToDecimal(item.Element("altim_in_hg").Value)
                                                               .ToString("N2") +
                                                           " in.");
                                            if (item.Element("sea_level_pressure_mb") != null)
                                            {
                                                weather.Append("(" +
                                                               AWDConvert.ToDecimal(
                                                                       item.Element("sea_level_pressure_mb").Value)
                                                                   .ToString("N1") +
                                                               " mb.)");
                                            }
                                            weather.Append(Environment.NewLine);
                                        }
                                        else
                                        {
                                            weather.Append("Pressure: Standard" + Environment.NewLine);
                                        }
                                        if (item.Element("sky_condition") != null &&
                                            item.Element("sky_condition").Attribute("cloud_base_ft_agl") != null &&
                                            item.Element("sky_condition").Attribute("sky_cover") != null)
                                        {
                                            weather.Append("Clouds: " +
                                                           item.Element("sky_condition")
                                                               .Attribute("cloud_base_ft_agl")
                                                               .Value +
                                                           " ft., " +
                                                           item.Element("sky_condition").Attribute("sky_cover").Value +
                                                           Environment.NewLine);
                                        }
                                        else
                                        {
                                            weather.Append("Clouds: Clear" + Environment.NewLine);
                                        }
                                        weather.Append(Environment.NewLine);

                                        if (item.Element("raw_text") != null)
                                        {
                                            weather.Append("Raw Text: " + item.Element("raw_text").Value +
                                                           Environment.NewLine);
                                        }

                                        if (!depdone && icao == dep)
                                        {
                                            depdone = true;
                                        }
                                        if (!destdone && icao == dest)
                                        {
                                            destdone = true;
                                        }
                                        txtRoute.Find(dep == icao ? dep : dest, RichTextBoxFinds.MatchCase);

                                        if (cat.Contains("LIFR"))
                                        {
                                            //                                _isIFR = true;
                                            txtRoute.SelectionBackColor = Color.Magenta;
                                            txtRoute.SelectionColor = Color.White;
                                        }
                                        else if (cat.Contains("IFR"))
                                        {
                                            //                                _isIFR = true;
                                            txtRoute.SelectionBackColor = Color.Red;
                                            txtRoute.SelectionColor = Color.White;
                                        }
                                        else if (cat.Contains("MVFR"))
                                        {
                                            txtRoute.SelectionBackColor = Color.Blue;
                                            txtRoute.SelectionColor = Color.White;
                                        }
                                        else if (cat.Contains("VFR"))
                                        {
                                            txtRoute.SelectionBackColor = Color.Green;
                                            txtRoute.SelectionColor = Color.White;
                                        }
                                        else
                                        {
                                            txtRoute.SelectionBackColor = Color.White;
                                            txtRoute.SelectionColor = Color.Black;
                                        }


                                        dict.Add(icao, weather.ToString().Replace("&deg;", "°"));
                                        if (returnVal.Length > 0)
                                        {
                                            returnVal.Append("|");
                                        }
                                        returnVal.Append(weather);
                                    }
                                }
                                if (depdone && destdone)
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unable to connect to the chart service.  Please check your internet connection and try again.");
                        }
                    }
                }
                return returnVal.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in FS Flight Builder, Updating Weather.", ex);
            }
        }

        internal static List<string> GetAllAircraft(ref List<string> aircraftFiles)
        {
            logger.Info($"Flight Sim = {FlightSim}");
            logger.Info($"Flight Sim Paths Count = {flightSimPaths.Count}");
            logger.Info($"Default Path = {flightSimPaths[FlightSim].AirplanesPath}");
            logger.Info($"Custom Path = {flightSimPaths[FlightSim].CustomFSPath}");
            List<string> aircraft = new List<string>();
            for (var idx = 0; idx < 2; idx++)
            {
                if (idx == 0) //Default FS Path
                {
                    if (!string.IsNullOrEmpty(flightSimPaths[FlightSim].DefaultFSPath))
                    {
                        var configFolders = Directory.GetFiles(flightSimPaths[FlightSim].DefaultFSPath, "thumbnail.jpg", SearchOption.AllDirectories);
                        foreach (var path in configFolders)
                        {
                            if (path.ToLower().Contains("simobjects\\airplanes"))
                            {
                                var parent = Directory.GetParent(Directory.GetParent(path).FullName);
                                if (!aircraftFiles.Contains(path))
                                {
                                    aircraftFiles.Add(path);
                                    aircraft.Add(parent.Name);
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(flightSimPaths[FlightSim].CustomFSPath))
                    {
                        var configFolders = Directory.GetFiles(flightSimPaths[FlightSim].CustomFSPath, "thumbnail.jpg", SearchOption.AllDirectories);
                        foreach (var path in configFolders)
                        {
                            if (path.ToLower().Contains("simobjects\\airplanes"))
                            {
                                var parent = Directory.GetParent(Directory.GetParent(path).FullName);
                                if (!aircraftFiles.Contains(path))
                                {
                                    aircraftFiles.Add(path);
                                    aircraft.Add(parent.Name);
                                }
                            }
                        }
                    }
                }
            }
            return aircraft;
        }

        //public static List<Aircraft> ParseAircraft(string thumbnailPath, int fltSim)
        public static List<Aircraft> ParseAircraft(List<string> thumbnailFiles)
        {
            logger.Info($"Parsing Aircraft. File count: {thumbnailFiles.Count}");
            var aircraft = new List<Aircraft>();
            foreach (var thumbnailFile in thumbnailFiles.Where(t => !t.Contains(".AirTraffic")))
            {
                try
                {
                    //The aircraft.cfg file (if it exists) will be in the parent of the thumbnail folder
                    var thumbnailPath = Directory.GetParent(thumbnailFile).FullName;
                    var mainPath = $"{Directory.GetParent(thumbnailPath).FullName}\\aircraft.cfg";
                    var idx = 0;
                    string pth;
                    string title = string.Empty;
                    bool titleFound = false;
                    var air = new Aircraft();
                    string[] parts_1;
                    string[] parts_2;
                    //one file per texture
                    if (File.Exists(mainPath))
                    {
                        var lines = File.ReadAllLines(mainPath);
                        foreach (string line in lines)
                        {
                            var cfgLine = line.ToLower();
                            var titleSearch = FlightSim == FlightSimType.MSFS ? "title =" : "title=";
                            var uitypeSearch = FlightSim == FlightSimType.MSFS ? "ui_type =" : "ui_type=";
                            if (cfgLine.StartsWith(titleSearch))
                            {
                                parts_1 = line.Split('=');
                                if (parts_1.Length > 1)
                                {
                                    parts_2 = parts_1[1].TrimStart().Split(';');
                                    title = parts_2[0].TrimEnd().Trim('\"');
                                    if (!titleFound)
                                    {
                                        air.Name = title;
                                    }
                                }
                            }
                            else if (cfgLine.StartsWith("texture="))
                            {
                                parts_1 = line.Split('=');
                                if (parts_1.Length > 1)
                                {
                                    parts_2 = parts_1[1].TrimStart().Split(';');
                                    var txtr = parts_2[0].TrimEnd().Trim('\"');

                                    if (thumbnailPath.ToLower().EndsWith($".{txtr}"))
                                    {
                                        //Found the correct texture for this plane
                                        air.Name = title;
                                        titleFound = true;
                                    }
                                }
                            }
                            else if (cfgLine.StartsWith(uitypeSearch))
                            {
                                parts_1 = line.Split('=');
                                var uit = string.Empty;
                                if (parts_1.Length > 1)
                                {
                                    parts_2 = parts_1[1].TrimStart().Split(';');
                                    uit = parts_2[0].TrimEnd().Trim('\"');
                                }
                                if (uit.ToLower().StartsWith("tt:"))
                                {
                                    idx = mainPath.ToLower().IndexOf("simobjects");
                                    pth = mainPath.Remove(idx) + "en-US.locPak";
                                    if (pth.Contains("-livery-"))
                                    {
                                        idx = pth.ToLower().IndexOf("-livery-");
                                        pth = mainPath.Remove(idx) + "\\en-US.locPak";
                                    }
                                    //open locPak and read the json
                                    if (File.Exists(pth))
                                    {
                                        using (StreamReader r = new StreamReader(pth))
                                        {
                                            var json = r.ReadToEnd();
                                            var props = JsonConvert.DeserializeObject<dynamic>(json);
                                            if (props != null && props.LocalisationPackage != null && props.LocalisationPackage.Strings != null)
                                            {
                                                air.UIType = props.LocalisationPackage.Strings["AIRCRAFT.UI_MODEL"];
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    air.UIType = uit;
                                }
                            }
                            else if (cfgLine.StartsWith("cruise_speed"))
                            {
                                var parts = line.Split('=');
                                if (parts.Length > 1)
                                {
                                    //Check for comments
                                    var parts2 = parts[1].Split('/');
                                    air.Airspeed = AWDConvert.ToDecimal(parts2[0]);
                                    var ac = DataHelper.GetAircraftByName(air.Name ?? "Unknown");
                                    if (ac != null)
                                    {
                                        air.ClimbRate = ac.ClimbRate != null ? ac.ClimbRate : 0;
                                        air.ClimbSpeed = ac.ClimbSpeed != null ? ac.ClimbSpeed : 0;
                                        air.DescentRate = ac.DescentRate != null ? ac.DescentRate : 0;
                                        air.DescentSpeed = ac.DescentSpeed != null ? ac.DescentSpeed : 0;
                                    }
                                }
                            }
                        }
                    }
                    //open the flight_model.cfg to get the airspeeds
                    pth = mainPath.Replace("aircraft.cfg", "flight_model.cfg");
                    if (File.Exists(pth))
                    {
                        var lines = File.ReadAllLines(pth);
                        foreach (string line in lines)
                        {
                            var cfgLine = line.ToLower();
                            if (cfgLine.StartsWith("cruise_speed ="))
                            {
                                parts_1 = line.Split('=');
                                if (parts_1.Length > 1)
                                {
                                    parts_2 = parts_1[1].TrimStart().Split(';');
                                    air.Airspeed = Convert.ToInt32(parts_2[0].TrimEnd());
                                }
                            }
                            else if (cfgLine.StartsWith("climb_speed ="))
                            {
                                parts_1 = line.Split('=');
                                if (parts_1.Length > 1)
                                {
                                    parts_2 = parts_1[1].TrimStart().Split(';');
                                    air.ClimbSpeed = Convert.ToInt32(parts_2[0].TrimEnd());
                                }
                            }
                            else if (cfgLine.StartsWith("spawn_cruise_altitude ="))
                            {
                                parts_1 = line.Split('=');
                                if (parts_1.Length > 1)
                                {
                                    parts_2 = parts_1[1].TrimStart().Split(';');
                                    air.ClimbRate = Convert.ToInt32(parts_2[0].TrimEnd());
                                }
                            }
                            else if (cfgLine.StartsWith("spawn_descent_altitude ="))
                            {
                                parts_1 = line.Split('=');
                                if (parts_1.Length > 1)
                                {
                                    parts_2 = parts_1[1].TrimStart().Split(';');
                                    air.ClimbRate = Convert.ToInt32(parts_2[0].TrimEnd());
                                }
                            }
                        }
                    }
                    var name = air.Name ?? $"{Directory.GetParent(thumbnailPath).Name}";
                    idx = thumbnailPath.LastIndexOf(".");
                    if (idx > -1)
                    {
                        name += $" ({thumbnailPath.Substring(idx + 1)})";
                    }
                    //var newAircraft = air;
                    try
                    {
                        if (!name.TrimStart(' ').StartsWith("("))
                        {
                            var newAircraft = new Aircraft
                            {
                                Airspeed = air.Airspeed,
                                ClimbRate = air.ClimbRate,
                                ClimbSpeed = air.ClimbSpeed,
                                DescentRate = air.DescentRate,
                                DescentSpeed = air.DescentSpeed,
                                FSType = (int)FlightSim,
                                UIType = air.UIType ?? Directory.GetParent(thumbnailPath).Name,
                                Name = name, //UIType + (texture)
                                Texture = thumbnailPath
                            };
                            aircraft.Add(newAircraft);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Error IN creating aircraft. Error is: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error($"Error IN Parsing. Error is: {ex.Message}");
                }

            }
            logger.Info($"Parsing Aircraft completed.");
            return aircraft.OrderBy(a => a.Name).ToList();
        }

        internal static bool SaveAircraft(List<Aircraft> aircraft, bool noSave = false)
        {
            //var dbAircraft = new List<Aircraft>();
            var db = new StringBuilder();
            try
            {
                using (var context = new FSFBDbConn(DBConnName, DataPath))
                {
                    if (!noSave)
                    {
                        //Fix the name and airspeed
                        FixAircraftData(context, aircraft);

                        context.Database.ExecuteSql($"DELETE FROM [Aircraft] WHERE FSType = {(int)Common.FlightSim}");
                        context.SaveChanges();

                        //Remove duplicates
                        //var noAircraftDups = aircraft.GroupBy(a => new { a.Name, a.Texture, a.FSType })
                        //                              //.Where(a => a.Count() == 1)
                        //                              .Select(a => a.First())
                        //                              .ToList();
                        context.Aircraft.AddRange(aircraft); // noAircraftDups);
                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"Err while saving aircraft. Error is {ex.Message}");
                return false;
            }
        }

        public static List<Aircraft> FixAircraftData(FSFBDbConn context, List<Aircraft> aircraft)
        {
            foreach (var acft in aircraft)
            {
                if (acft.UIType.StartsWith("$$:"))
                {
                    acft.UIType = acft.UIType.Substring(3).Replace(" ", "");
                }
                if (acft.UIType.ToLower().Contains("_livery"))
                {
                    acft.UIType = acft.UIType.Substring(0, acft.UIType.ToLower().IndexOf("_livery"));
                }

                var uiType = acft.UIType.ToLower();
                if (acft.Airspeed == 0)
                {
                    //See if another aircraft of this type has an airspeed
                    acft.Airspeed = context.Aircraft.FirstOrDefault(a => a.Airspeed > 0 && a.UIType.ToLower() == uiType && a.FSType == acft.FSType)?.Airspeed ?? 0;
                }
                if (acft.DescentSpeed == 0)
                {
                    //See if another aircraft of this type has an descentspeed
                    acft.DescentSpeed = context.Aircraft.FirstOrDefault(a => a.DescentSpeed > 0 && a.UIType.ToLower() == uiType && a.FSType == acft.FSType)?.DescentSpeed ?? 0;
                }
                if (acft.DescentRate == 0)
                {
                    //See if another aircraft of this type has an descentrate
                    acft.DescentRate = context.Aircraft.FirstOrDefault(a => a.DescentRate > 0 && a.UIType.ToLower() == uiType && a.FSType == acft.FSType)?.DescentRate ?? 0;
                }
                if (acft.ClimbSpeed == 0)
                {
                    //See if another aircraft of this type has an descentspeed
                    acft.ClimbSpeed = context.Aircraft.FirstOrDefault(a => a.ClimbSpeed > 0 && a.UIType.ToLower() == uiType && a.FSType == acft.FSType)?.ClimbSpeed ?? 0;
                }
                if (acft.ClimbRate == 0)
                {
                    //See if another aircraft of this type has an descentrate
                    acft.ClimbRate = context.Aircraft.FirstOrDefault(a => a.ClimbRate > 0 && a.UIType.ToLower() == uiType && a.FSType == acft.FSType)?.ClimbRate ?? 0;
                }
            }
            return aircraft;
        }

        internal static StringBuilder BuildExceptionMessage(Exception ex)
        {
            var result = new StringBuilder();
            result.Append($"{Environment.NewLine}Error is: {ex.Message + Environment.NewLine}.  Stack Trace: {ex.StackTrace}");
            var inner = ex.InnerException;
            while (inner != null)
            {
                result.Append($"{Environment.NewLine}Inner exception is: {inner.Message + Environment.NewLine}.  Stack Trace: {inner.StackTrace}");
                inner = inner.InnerException;
            }
            return result;
        }

        internal static string FormatSpeed(decimal spd)
        {
            const string fmt = "0000";
            return spd.ToString(fmt);
        }

        internal static FlightSimType GetFSType()
        {
            var fsType = Properties.Settings.Default.FlightSim;
            switch (fsType)
            {
                case "FSX":
                    return FlightSimType.FSX;
                case "FSXSE":
                    return FlightSimType.FSXSE;
                case "P3D":
                    return FlightSimType.P3D;
                case "MSFS":
                    return FlightSimType.MSFS;
                default:
                    return FlightSimType.Unknown;
            }
        }

        public static string GetMsfsOfficialPath(string basePath)
        {
            // Also check subfolders and layout file to avoid confusion if folders from other installations remain
            // Look first for Steam since some might have remains from MS subscription around
            var isSteam = File.Exists($"{basePath}\\Official\\Steam\\fs-base\\layout.json");
            var isOneStore = File.Exists($"{basePath}\\Official\\OneStore\\fs-base\\layout.json");

            if (isSteam)
                return $"{basePath}\\Official\\Steam";
            else if (isOneStore)
                return $"{basePath}\\Official\\OneStore";
            else
            {
                loggingUtil.logger.Warn($"isSteam is false, isOneStore is false");
                return string.Empty;
            }
        }

        internal static string GetMsfsCommunityPath(string basePath)
        {
            if (Directory.Exists($"{basePath}\\Community"))
                return $"{basePath}\\Community";
            else
            {
                return string.Empty;
            }
        }

        private static void ConvertSettings()
        {
            try
            {
                //Convert to INI file to the new settings
                var ini = new IniFile(appPath + @"\FSFlightBuilder.ini");
                //Charts
                WriteSetting(ini, "Charts", "Service");

                //Paths
                WriteSetting(ini, "Paths", "FSXSEAirplanesPath", "FSXSEAirplanesPath");
                WriteSetting(ini, "Paths", "P3DAirplanesPath", "P3DAirplanesPath");
                WriteSetting(ini, "Paths", "FSXPath", "FSXDefaultFSPath");
                WriteSetting(ini, "Paths", "FSXFPPath");
                WriteSetting(ini, "Paths", "FSXSEPath", "FSXSEDefaultFSPath");
                WriteSetting(ini, "Paths", "FSXSEFPPath");
                WriteSetting(ini, "Paths", "P3DPath", "P3DDefaultFSPath");
                WriteSetting(ini, "Paths", "P3DFPPath");
                WriteSetting(ini, "Paths", "FSXAirplanesPath", "FSXAirplanesPath");

                //Versions
                WriteSetting(ini, "Versions", "FSXSEVersion");
                WriteSetting(ini, "Versions", "P3DVersion");
                WriteSetting(ini, "Versions", "FSXVersion");
                WriteSetting(ini, "Versions", "FlightSim");
                WriteSetting(ini, "Versions", "UpdateOnStart");
                WriteSetting(ini, "Versions", "DeleteImagesOnClose");
                WriteSetting(ini, "Versions", "AIRAC");

                //FlightTime
                WriteSetting(ini, "FlightTime", "UseSystem");
                WriteSetting(ini, "FlightTime", "LastFlight");

                //Chooser
                WriteSetting(ini, "Chooser", "ChooserSettings");

                //Plan
                WriteSetting(ini, "Plan", "IncludeTOD");

                Properties.Settings.Default.Save();
                try
                {
                    if (File.Exists(appPath + @"\FSFlightBuilder.ini.bak"))
                    {
                        var path = Path.GetRandomFileName();
                        File.Move(appPath + @"\FSFlightBuilder.ini.bak", appPath + @"\path");
                    }
                    File.Move(appPath + @"\FSFlightBuilder.ini", appPath + @"\FSFlightBuilder.ini.bak");
                }
                catch
                {
                    File.Move(appPath + @"\FSFlightBuilder.ini", appPath + @"\FSFlightBuilder.ini.awd");
                }
                logger.Info($"Successfully converted the ini file.");
            }
            catch (Exception ex)
            {
                logger.Error($"Error converting the ini file. The error is {ex.Message}");
            }
        }

        private static void WriteSetting(IniFile ini, string section, string key, string settingName = "")
        {
            var val = ini.IniReadValue(section, key);
            if (string.IsNullOrEmpty(settingName))
            {
                settingName = key;
            }
            if (!string.IsNullOrEmpty(val))
            {
                //Copy it to Default Path and delete it
                if (Properties.Settings.Default[settingName].GetType() == typeof(bool))
                {
                    Properties.Settings.Default[settingName] = Convert.ToBoolean(val);
                }
                else
                {
                    Properties.Settings.Default[settingName] = val;
                }
            }
        }
    }
}
