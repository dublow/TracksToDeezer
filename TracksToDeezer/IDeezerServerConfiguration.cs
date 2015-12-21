using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracksToDeezer
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
