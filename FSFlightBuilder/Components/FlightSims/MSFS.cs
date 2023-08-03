using FSFlightBuilder.Entities;
using System;
using System.IO;
using System.Windows.Forms;

namespace FSFlightBuilder.Components.FlightSims
{
    internal static class MSFS
    {
        internal static FSPaths GetInstallPaths(FSPaths fsPaths)
        {
            try
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (Directory.Exists($"{appDataFolder}\\Packages\\Microsoft.FlightSimulator_8wekyb3d8bbwe\\LocalCache"))
                {
                    fsPaths.AppDataPath = $"{appDataFolder}\\Packages\\Microsoft.FlightSimulator_8wekyb3d8bbwe\\LocalCache".Trim('\0');
                    //Open the appdatapath UserCfg.opt to get the main FSX main data folder location
                    if (File.Exists($"{fsPaths.AppDataPath}\\UserCfg.opt"))
                    {
                        if (string.IsNullOrEmpty(fsPaths.DefaultFSPath))
                        {
                            MessageBox.Show("MSFS found.", "New Flight Sim Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        string dir = string.Empty;
                        //Read the InstalledPackagesPath setting
                        using (StreamReader file = new StreamReader($"{fsPaths.AppDataPath}\\UserCfg.opt"))
                        {
                            string ln;
                            while ((ln = file.ReadLine()) != null)
                            {
                                if (ln.StartsWith("InstalledPackagesPath"))
                                {
                                    string[] parts = ln.Split(' ');
                                    if (parts.Length > 1)
                                    {
                                        var path = parts[1].Trim('"');
                                        DirectoryInfo fi = new DirectoryInfo(path);
                                        if (Directory.Exists(fi.FullName))
                                        {
                                            dir = fi.FullName.Trim('"');
                                            break;
                                        }
                                        else
                                        {
                                            Common.logger.Warn($"{fi.FullName} does not exist or is not a directory");
                                        }
                                    }
                                }
                            }
                            file.Close();
                        }

                        // Official/Steam or Official/OneStore is required =================
                        if (!string.IsNullOrEmpty(dir))
                        {
                            fsPaths.DefaultFSPath = Common.GetMsfsOfficialPath(dir);
                            if (string.IsNullOrEmpty(fsPaths.DefaultFSPath))
                            {
                                dir = string.Empty;
                                Common.logger.Warn($"MSFS official path not found");
                            }

                            fsPaths.CustomFSPath = Common.GetMsfsCommunityPath(dir);
                            if (string.IsNullOrEmpty(fsPaths.CustomFSPath))
                            {
                                Common.logger.Warn($"MSFS community path not found");
                            }
                        }
                    }

                    fsPaths.FPPath = GetFPPath(fsPaths.DefaultFSPath, fsPaths.FPPath);
                    fsPaths.AirplanesPath = fsPaths.DefaultFSPath; // GetAircraftPath(fsPaths.DefaultFSPath, fsPaths.AirplanesPath);
                    fsPaths.Installed = true;
                    Common.logger.Info("MSFS location found. {0}", fsPaths.DefaultFSPath);
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
                Directory.Exists(fsPath + @"\LocalState\MISSIONS\Custom"))
            {
                if (!Directory.Exists(fsPath + @"\LocalState\MISSIONS\Custom\FS Flight Builder"))
                {
                    Directory.CreateDirectory(fsPath + @"\LocalState\MISSIONS\Custom\FS Flight Builder");
                }
                fpPath = fsPath.TrimEnd('\\') + @"\LocalState\MISSIONS\Custom\FS Flight Builder";
                Common.logger.Info("MSFS Flight Plan location found. {0}", fpPath);
            }
            return fpPath;
        }

        internal static string GetAircraftPath(string fsPath, string aircraftPath)
        {
            if (string.IsNullOrEmpty(aircraftPath))
            {
                aircraftPath = fsPath.TrimEnd('\\') + @"\Official\OneStore";
                Common.logger.Info("MSFS Aircraft location found. {0}", aircraftPath);
            }
            return aircraftPath;
        }

        internal static string GetFSVersion(string fsPath)
        {
            if (!string.IsNullOrEmpty(fsPath))
            {
            }
            return string.Empty;
        }
    }
}
