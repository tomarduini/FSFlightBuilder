using NLog;
using NLog.Targets;
using System.IO;
using System.Windows.Forms;

namespace FSFlightBuilder.Logger
{
    internal static class LoggingConfig
    {
        public static void ConfigureLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var dataPath =
                Path.GetDirectoryName(Application.ExecutablePath).ToLower().Replace(@"bin\debug", string.Empty).Replace(@"bin\release", string.Empty) +
                @"Data";
            var logPath = dataPath.Replace("\\Data", "\\Logs\\");
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            config.Variables.Add("LogHome", logPath);
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"{logPath}\\fsflightbuilder.log", Layout = "${longdate}|${callsite}|${message}|${exception}", ArchiveFileName = "${LogHome}/Archive/Info-${shortdate}.awd", MaxArchiveFiles = 5, ArchiveEvery = FileArchivePeriod.Day };
            var errorlog = new NLog.Targets.FileTarget("errorlog") { FileName = $"{logPath}\\fsflightbuilder_error.log", Layout = "${longdate}|${callsite}|${message}|${exception}", ArchiveFileName = "${LogHome}/Archive/Error-${shortdate}.awd", MaxArchiveFiles = 5, ArchiveEvery = FileArchivePeriod.Day };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Warn, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Error, LogLevel.Fatal, errorlog);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }

    }
}
