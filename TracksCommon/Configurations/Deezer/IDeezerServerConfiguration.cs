using System.Collections.Generic;
using TracksCommon.Entities;

namespace TracksCommon.Configurations.Deezer
{
    public interface IDeezerServerConfiguration
    {
        string ServiceName { get; }
        string ConnectionString { get; }
        string AppId { get; }
        string SecretId { get; }
        string Callback { get; }
        string Playlist { get; }
        List<string> Radios { get; }

        Dictionary<Endpoint, string> Endpoints { get; }
    }
}
