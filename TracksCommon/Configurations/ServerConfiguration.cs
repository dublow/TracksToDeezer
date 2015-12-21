using System.Configuration;

namespace TracksCommon.Configurations
{
    public class ServerConfiguration : IServerConfiguration
    {
        private static readonly Configuration Configuration;

        static ServerConfiguration()
        {
            Configuration = (Configuration)ConfigurationManager.GetSection("radio");
        }

        public string ServiceName
        {
            get { return Configuration.ServiceName; }
        }


        public string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Radio"].ConnectionString; }
        }

        public string Timer { get { return Configuration.Timer; } }
        public string RadioUrl { get { return Configuration.RadioUrl; } }
        public string RegexArtist { get { return Configuration.RegexArtist; } }
        public string RegexTitle { get { return Configuration.RegexTitle; } }
        public bool Debug { get { return Configuration.Debug == "true"; } }
    }
}
