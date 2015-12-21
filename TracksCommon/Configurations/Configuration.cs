using System.Configuration;

namespace TracksCommon.Configurations
{
    public class Configuration : ConfigurationSection
    {
        [ConfigurationProperty("serviceName")]
        public string ServiceName
        {
            get { return (string)this["serviceName"]; }
        }

        [ConfigurationProperty("timer")]
        public string Timer
        {
            get
            {
                return (string)this["timer"];
            }
        }

        [ConfigurationProperty("radiourl")]
        public string RadioUrl
        {
            get
            {
                return (string)this["radiourl"];
            }
        }

        [ConfigurationProperty("regexartist")]
        public string RegexArtist
        {
            get
            {
                return (string)this["regexartist"];
            }
        }

        [ConfigurationProperty("regextitle")]
        public string RegexTitle
        {
            get
            {
                return (string)this["regextitle"];
            }
        }

        [ConfigurationProperty("debug")]
        public string Debug
        {
            get
            {
                return (string)this["debug"];
            }
        }
    }
}
