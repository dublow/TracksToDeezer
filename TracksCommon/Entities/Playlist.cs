using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracksCommon.Entities
{
    public class Playlist : IDeserializer
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}
