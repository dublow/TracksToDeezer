using System.Configuration;

namespace TracksCommon.Configurations.Deezer
{
    public class DeezerSection : ConfigurationSection
    {
        [ConfigurationProperty("serviceName")]
        public string ServiceName
        {
            get { return (string)this["serviceName"]; }
        }

        [ConfigurationProperty("appId")]
        public string AppId
        {
            get { return (string)this["appId"]; }
        }

        [ConfigurationProperty("secretId")]
        public string SecretId
        {
            get { return (string)this["secretId"]; }
        }

        [ConfigurationProperty("callback")]
        public string Callback
        {
            get { return (string)this["callback"]; }
        }

        [ConfigurationProperty("playlist")]
        public string Playlist
        {
            get { return (string)this["playlist"]; }
        }

        [ConfigurationProperty("radios")]
        public string Radios
        {
            get
            {
                return (string)this["radios"];
            }
        }

        [ConfigurationProperty("endpoints", IsRequired = true, IsDefaultCollection = true)]
        public DeezerEndpointCollection Endpoints
        {
            get { return (DeezerEndpointCollection)this["endpoints"]; }
            set { this["endpoints"] = value; }
        }
    }
}
