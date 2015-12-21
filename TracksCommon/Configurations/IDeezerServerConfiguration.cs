using System.Collections.Generic;

namespace TracksCommon.Configurations
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
    }
}
