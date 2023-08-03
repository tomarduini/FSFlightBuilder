using Ionic.Zip;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
//using teboweb;

namespace FSFlightBuilder.Components
{
    internal class Update
    {

        /// <summary>Get update and version information from specified online file - returns a List</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="versionFile">Name of the pipe| delimited version file to download</param>
        /// <param name="resourceDownloadFolder">Folder on the local machine to download the version file to</param>
        /// <param name="startLine">Line number, of the version file, to read the version information from</param>
        /// <returns>List containing the information from the pipe delimited version file</returns>
        public static List<string> getUpdateInfo(string downloadsURL, string versionFile, string resourceDownloadFolder, int startLine)
        {
            //create download folder if it does not exist
            if (!Directory.Exists(resourceDownloadFolder))
            {

                Directory.CreateDirectory(resourceDownloadFolder);

            }

            //let's try and download update information from the web
            var updateChecked = webData.downloadFromWeb(downloadsURL, versionFile, resourceDownloadFolder);

            //if the download of the file was successful
            if (updateChecked)
            {
                //get information out of download info file
                return populateInfoFromWeb();
            }
            //there is a chance that the download of the file was not successful
            else
            {

                return null;

            }

        }



        /// <summary>Download file from the web immediately</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="filename">Name of the file to download</param>
        /// <param name="downloadTo">Folder on the local machine to download the file to</param>
        /// <param name="unzip">Unzip the contents of the file</param>
        /// <returns>Void</returns>
        public static void installUpdateNow(string downloadsURL, string filename, string downloadTo, bool unzip)
        {
            webData.downloadFromWeb(downloadsURL, filename, downloadTo);
            if (unzip)
            {
                unZip(downloadTo + filename, downloadTo);
            }
        }


        /// <summary>Starts the update application passing across relevant information</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="filename">Name of the file to download</param>
        /// <param name="destinationFolder">Folder on the local machine to download the file to</param>
        /// <param name="processToEnd">Name of the process to end before applying the updates</param>
        /// <param name="postProcess">Name of the process to restart</param>
        /// <param name="startupCommand">Command line to be passed to the process to restart</param>
        /// <param name="updater"></param>
        /// <returns>Void</returns>
        public static void installUpdateRestart(string downloadsURL, string filename, string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater)
        {
            var cmdLn = "|downloadFile|" + filename;
            cmdLn += "|URL|" + downloadsURL;
            cmdLn += "|destinationFolder|" + destinationFolder;
            cmdLn += "|processToEnd|" + processToEnd;
            cmdLn += "|postProcess|" + postProcess;
            cmdLn += "|command|" + @" / " + startupCommand;

            var startInfo = new ProcessStartInfo
            {
                FileName = updater,
                Arguments = cmdLn
            };
            Process.Start(startInfo);

        }

        /// <summary>Starts the update application passing across relevant information</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="filename">Name of the file to download</param>
        /// <param name="destinationFolder">Folder on the local machine to download the file to</param>
        /// <param name="processToEnd">Name of the process to end before applying the updates</param>
        /// <param name="postProcess">Name of the process to restart</param>
        /// <param name="startupCommand">Command line to be passed to the process to restart</param>
        /// <param name="updater"></param>
        /// <returns>Void</returns>
        public static void installUpdateFix(string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater)
        {
            var cmdLn = "|destinationFolder|" + destinationFolder.Replace(" ", "~");
            cmdLn += "|processToEnd|" + processToEnd;
            cmdLn += "|postProcess|" + postProcess.Replace(" ", "~");
            cmdLn += "|command|" + @" / " + startupCommand;

            var startInfo = new ProcessStartInfo
            {
                FileName = updater,
                Arguments = cmdLn
            };
            Process.Start(startInfo);

        }



        private static List<string> populateInfoFromWeb()
        {

            List<string> tempList = new List<string>();
            if (File.Exists("fsflightbuilderupdate.xml"))
            {
                foreach (var strline in File.ReadAllLines("fsflightbuilderupdate.xml"))
                {
                    string[] parts = strline.Split('|');
                    tempList.AddRange(parts);
                    return tempList;
                }
            }


            return null;

        }




        private static void unZip(string file, string unZipTo)
        {
            try
            {

                // Specifying Console.Out here causes diagnostic msgs to be sent to the Console
                // In a WinForms or WPF or Web app, you could specify nothing, or an alternate
                // TextWriter to capture diagnostic messages. 

                using (var zip = ZipFile.Read(file))
                {
                    // This call to ExtractAll() assumes:
                    //   - none of the entries are password-protected.
                    //   - want to extract all entries to current working directory
                    //   - none of the files in the zip already exist in the directory;
                    //     if they do, the method will throw.
                    zip.ExtractAll(unZipTo);
                }

            }
            catch
            {
                //
            }
        }

        /// <summary>Updates the update application by renaming prefixed files</summary>
        /// <param name="updaterPrefix">Prefix of files to be renamed</param>
        /// <param name="containingFolder">Folder on the local machine where the prefixed files exist</param>
        /// <returns>Void</returns>
        public static void updateMe(string updaterPrefix, string containingFolder)
        {

            var dInfo = new DirectoryInfo(containingFolder);
            var updaterFiles = dInfo.GetFiles(updaterPrefix + "*");
            foreach (var file in updaterFiles)
            {
                var newFile = containingFolder + file.Name;
                var origFile = containingFolder + @"\" + file.Name.Substring(updaterPrefix.Length, file.Name.Length - updaterPrefix.Length);

                if (File.Exists(origFile)) { File.Delete(origFile); }

                File.Move(newFile, origFile);
            }
        }

    }
}

