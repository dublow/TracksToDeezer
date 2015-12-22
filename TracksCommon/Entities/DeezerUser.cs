using System.Collections.Generic;
using Newtonsoft.Json;

namespace TracksCommon.Entities
{
    public class DeezerUser : IDeserializer
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
