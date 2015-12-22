using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TracksCommon.Configurations.Deezer;
using TracksCommon.Entities;

namespace TracksToDeezer.Tools
{
    public class ServerConfiguration : IDeezerServerConfiguration
    {
        private static readonly DeezerSection Section;

        static ServerConfiguration()
        {
            Section = (DeezerSection)ConfigurationManager.GetSection("trackstodeezer");
        }

        public string ServiceName { get { return Section.ServiceName; } }
        public string AppId { get { return Section.AppId; } }
        public string SecretId { get { return Section.SecretId; } }
        public string Callback { get { return Section.Callback; } }
        public string Playlist { get { return Section.Playlist; } }
        public List<string> Radios
        {
            get
            {
                return Section.Radios
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();
            }
        }

        public string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Tools"].ConnectionString; }
        }

        public Dictionary<Endpoint, string> Endpoints
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
