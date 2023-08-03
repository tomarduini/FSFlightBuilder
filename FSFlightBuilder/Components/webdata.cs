using System;
using System.Net.Http;

namespace FSFlightBuilder.Components
{

    public delegate void BytesDownloadedEventHandler(ByteArgs e);

    public class ByteArgs : EventArgs
    {
        public int downloaded { get; set; }

        public int total { get; set; }
    }

    class webData
    {

        public static event BytesDownloadedEventHandler bytesDownloaded;

        public static bool downloadFromWeb(string URL, string file, string targetFolder)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = $"https://www.arduiniwebdevelopment.com/updates/fsflightbuilderupdate.xml";
                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var responseContent = response.Content;
                            var updateResult = responseContent.ReadAsStringAsync().Result;
                            System.IO.File.WriteAllText("fsflightbuilderupdate.xml", updateResult);
                            return true;
                        }
                        catch (Exception e)
                        {
                            Common.logger.Error($"Error downloading the fsflightbuilderupdate xml file. Error is {e.Message}");
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Common.logger.Error($"Error downloading the fsflightbuilderupdate xml file. Error is {e.Message}");
            }
            return false;
        }

        public static void OnBytesDownloaded(ByteArgs e)
        {
            bytesDownloaded?.Invoke(e);
        }
    }
}
