using FSFlightBuilder.Entities;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FSFlightBuilder.Components.FlightSims
{
    internal static class P3D
    {
        internal static FSPaths GetInstallPaths(FSPaths fsPaths)
        {
            try
            {
                var key = Registry.CurrentUser.OpenSubKey(@"Software\Lockheed Martin\Prepar3D v5") ??
                            Registry.CurrentUser.OpenSubKey(@"Software\Lockheed Martin\Prepar3D v4") ??
                            Registry.CurrentUser.OpenSubKey(@"Software\Lockheed Martin\Prepar3D v3") ??
                          Registry.CurrentUser.OpenSubKey(@"Software\Lockheed Martin\Prepar3D v2") ??
                           Registry.CurrentUser.OpenSubKey(@"Software\LockheedMartin\Prepar3D");
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
                    else
                    {
                        if (string.IsNullOrEmpty(fsPaths.DefaultFSPath))
                        {
                            MessageBox.Show("Prepar3D found.", "New Flight Sim Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        fsPaths.DefaultFSPath = fsPath;
                        fsPaths.CustomFSPath = GetCustomFSPath(fsPath);
                        fsPaths.AppDataPath = GetAppDataPath();
                        fsPaths.FPPath = GetFPPath();
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
                Common.logger.Error("Error getting P3D registry. Error is: {0}", ex.Message);
            }

            return fsPaths;
        }

        internal static string GetAppDataPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            for (var i = 5; i > 1; i--)
            {
                if (Directory.Exists($"{appDataPath}\\Lockheed Martin\\Prepar3D v{i}"))
                {
                    return $"{appDataPath}\\Lockheed Martin\\Prepar3D v{i}";
                }
            }
            if (Directory.Exists($"{appDataPath}\\Lockheed Martin\\Prepar3D"))
            {
                return $"{appDataPath}\\Lockheed Martin\\Prepar3D";
            }
            Common.logger.Info("P3D App Data location not found.");
            return string.Empty;
        }

        internal static string GetCustomFSPath(string fsPath)
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            for (var i = 5; i > 1; i--)
            {
                if (Directory.Exists($"{myDocuments}\\Prepar3D v{i} Add-ons"))
                {
                    return $"{myDocuments}\\Prepar3D v{i} Add-ons";
                }
            }
            if (Directory.Exists($"{myDocuments}\\Prepar3D Add-ons"))
            {
                return $"{myDocuments}\\Prepar3D Add-ons";
            }

            if (Directory.Exists($"{fsPath}\\Addon Scenery"))
            {
                return $"{fsPath}\\Addon Scenery";
            }

            Common.logger.Info("P3D custom folder not found.");
            return string.Empty;
        }

        internal static string GetFPPath()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            for (var i = 5; i > 1; i--)
            {
                if (Directory.Exists($"{myDocuments}\\Prepar3D v{i} Files"))
                {
                    return $"{myDocuments}\\Prepar3D v{i} Files";
                }
            }
            if (Directory.Exists($"{myDocuments}\\Prepar3D Files"))
            {
                return $"{myDocuments}\\Prepar3D Files";
            }
            Common.logger.Info("P3D Flight Plan location found.");
            return string.Empty;
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
