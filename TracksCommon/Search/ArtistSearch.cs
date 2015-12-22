using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;
using TracksCommon.Filters;

namespace TracksCommon.Search
{
    public class ArtistSearch : BaseFilter, ISearch
    {
        private readonly string url;

        public ArtistSearch(string url, IEnumerable<IFilter> filters) 
            : base(filters, "Search: ByArtist")
        {
            this.url = url;
        }

        public string GetUrl(string artist, string title)
        {
            return string.Format(url, artist);
        }
    }
}
