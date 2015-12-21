using System.Collections.Generic;
using Newtonsoft.Json;

namespace TracksCommon.Entities
{
    public class DeezerPlaylist
    {
        public IEnumerable<Playlist> Data { get; set; }
    }
}
