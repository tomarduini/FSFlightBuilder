using FSFlightBuilder.Entities;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FSFlightBuilder.Components.FlightSims
{
    internal static class FSX
    {
        internal static FSPaths GetInstallPaths(FSPaths fsPaths)
        {
            try
            {
                var key =
                    Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Microsoft Games\Flight Simulator\10.0");
                if (key != null)
                {
                    var fsPath = key.GetValue("AppPath").ToString().Trim('\0');
                    if (string.IsNullOrEmpty(fsPath))
                    {
                        fsPaths.DefaultFSPath = string.Empty;
                        fsPaths.AppDataPath = string.Empty;
                        fsPaths.CustomFSPath = string.Empty;
                        fsPaths.FPPath = string.Empty;
                        fsPaths.AirplanesPath = string.Empty;
                        fsPaths.Installed = false;
                    }
                    else if (!File.Exists($"{fsPath}\\steam_api.dll"))
                    {
                        if (string.IsNullOrEmpty(fsPaths.DefaultFSPath))
                        {
                            MessageBox.Show("FSX found.", "New Flight Sim Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        fsPaths.DefaultFSPath = fsPath;
                        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        fsPaths.AppDataPath = Directory.Exists(appDataPath + "\\Microsoft\\FSX") ?
                            appDataPath + "\\Microsoft\\FSX" :
                            string.Empty;
                        fsPaths.CustomFSPath = Directory.Exists(fsPath + "\\Addon Scenery") ?
                            fsPath + "\\Addon Scenery" :
                            string.Empty;
                        fsPaths.FPPath = GetFPPath(fsPath, fsPaths.FPPath);
                        fsPaths.AirplanesPath = GetAircraftPath(fsPath, fsPaths.AirplanesPath);
                        fsPaths.Installed = true;
                        Common.logger.Info("FSX location found. {0}", fsPath);
                    }
                }
                else
                {
                    fsPaths.DefaultFSPath = string.Empty;
                    fsPaths.AppDataPath = string.Empty;
                    fsPaths.CustomFSPath = string.Empty;
                    fsPaths.FPPath = string.Empty;
                    fsPaths.AirplanesPath = string.Empty;
                    fsPaths.Installed = false;
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error("Error getting FSX registry. Error is: {0}", ex.Message);
            }

            return fsPaths;
        }

        internal static string GetFPPath(string fsPath, string fpPath)
        {
            if (string.IsNullOrEmpty(fpPath) && Directory.Exists(fsPath) &&
                Directory.Exists(fsPath + @"\Missions"))
            {
                if (!Directory.Exists(fsPath + @"\Missions\FS Flight Builder"))
                {
                    Directory.CreateDirectory(fsPath + @"\Missions\FS Flight Builder");
                }
                fpPath = fsPath.TrimEnd('\\') + @"\Missions\FS Flight Builder";
                Common.logger.Info("FSX Flight Plan location found. {0}", fpPath);
            }
            return fpPath;
        }

        internal static string GetAircraftPath(string fsPath, string aircraftPath)
        {
            if (string.IsNullOrEmpty(aircraftPath))
            {
                aircraftPath = fsPath.TrimEnd('\\') + @"\SimObjects\Airplanes";
                Common.logger.Info("FSX Aircraft location found. {0}", aircraftPath);
            }
            return aircraftPath;
        }

        internal static string GetFSVersion(string fsPath)
        {
            if (!string.IsNullOrEmpty(fsPath))
            {
                try
                {
                    if (File.Exists(fsPath.TrimEnd('\\') + @"\fsx.exe"))
                    {
                        var versionInfo = FileVersionInfo.GetVersionInfo(fsPath.TrimEnd('\\') + @"\fsx.exe");
                        var idx = versionInfo.ProductVersion.IndexOf(" ", StringComparison.Ordinal);
                        var fsxAppVersion = idx > -1
                            ? versionInfo.ProductVersion.Substring(0, idx)
                            : versionInfo.ProductVersion;
                        Common.logger.Info("FSX Version: {0}", fsxAppVersion);
                        return fsxAppVersion;
                    }
                }
                catch
                {
                }
            }
            return string.Empty;
        }
    }
}
