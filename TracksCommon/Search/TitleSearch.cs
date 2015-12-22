using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;
using TracksCommon.Filters;

namespace TracksCommon.Search
{
    public class TitleSearch : BaseFilter, ISearch
    {
        private readonly string url;
        
        public TitleSearch(string url, IEnumerable<IFilter> filters) 
            : base(filters, "Search: ByTitle")
        {
            this.url = url;
        }

        public string GetUrl(string artist, string title)
        {
            return string.Format(url, title);
        }
    }
}
