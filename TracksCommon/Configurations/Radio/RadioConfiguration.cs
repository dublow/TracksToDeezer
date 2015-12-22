using System.Configuration;

namespace TracksCommon.Configurations.Radio
{
    public class RadioConfiguration : IRadioConfiguration
    {
        private static readonly RadioSection Configuration;

        static RadioConfiguration()
        {
            Configuration = (RadioSection)ConfigurationManager.GetSection("radio");
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
