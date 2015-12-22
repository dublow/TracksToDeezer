using System.Collections.Generic;

namespace TracksCommon.Entities
{
    public class DeezerSearch : IDeserializer
    {
        public IEnumerable<DeezerSearchItem> Data { get; set; }
    }
}
