using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Configurations;
using TracksCommon.Entities;

namespace TracksToDeezer
{
    public class ServerConfiguration : IDeezerServerConfiguration
    {
        private static readonly DeezerConfiguration Configuration;

        static ServerConfiguration()
        {
            Configuration = (DeezerConfiguration)ConfigurationManager.GetSection("trackstodeezer");
        }

        public string ServiceName { get { return Configuration.ServiceName; } }
        public string AppId { get { return Configuration.AppId; } }
        public string SecretId { get { return Configuration.SecretId; } }
        public string Callback { get { return Configuration.Callback; } }
        public string Playlist { get { return Configuration.Playlist; } }
        public List<string> Radios
        {
            get
            {
                return Configuration.Radios
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();
            }
        }

        public Dictionary<Endpoint, string> EndpointUrls { get; private set; }

        public string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Deezer"].ConnectionString; }
        }
    }
}
