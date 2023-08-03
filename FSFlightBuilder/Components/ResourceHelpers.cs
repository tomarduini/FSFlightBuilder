using System.IO;

namespace FSFlightBuilder.Components
{
    public class ResourceHelpers
    {
        public string GetResourceTextFile(string filename)
        {
            using (Stream stream = GetType().Assembly.
                       GetManifestResourceStream("FSFlightBuilder.Data.Templates." + filename))
            {
                if (stream != null)
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        return sr.ReadToEnd();
                    }
                }
                return string.Empty;
            }
        }

        public Stream GetResourceFile(string filename)
        {
            return GetType().Assembly.
                GetManifestResourceStream("FSFlightBuilder.Data.Templates." + filename);
        }

        public StreamReader GetResourceStream(string filename)
        {
            using (Stream stream = GetType().Assembly.
                       GetManifestResourceStream("FSFlightBuilder.Data.Templates." + filename))
            {
                return new StreamReader(stream);
            }
        }

    }
}
