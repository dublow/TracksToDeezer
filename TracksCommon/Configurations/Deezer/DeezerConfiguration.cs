using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TracksCommon.Entities;

namespace TracksCommon.Configurations.Deezer
{
    public class DeezerConfiguration : IDeezerServerConfiguration
    {
        private static readonly DeezerSection Section;

        static DeezerConfiguration()
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
            get { return ConfigurationManager.ConnectionStrings["Deezer"].ConnectionString; }
        }

        public Dictionary<Endpoint, string> Endpoints
        {
            get
            {
                return Section.Endpoints.ToDictionary(x => (Endpoint)Enum.Parse(typeof(Endpoint), x.Name), v => v.Value);
            }
        }
    }
}
