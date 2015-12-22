using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksCommon.Filters
{
    public abstract class BaseFilter
    {
        private readonly IEnumerable<IFilter> filters;
        private readonly string message;

        public BaseFilter(IEnumerable<IFilter> filters, string message)
        {
            this.filters = filters;
            this.message = message;
        }

        public DeezerSearchItem Filtering(DeezerSearch deezerSearch, string artist, string title)
        {
            DeezerSearchItem search = null;
            foreach (var filter in filters)
            {
                search = filter.Create(deezerSearch, artist, title, message);
                if (search != null)
                    break;
            }

            return search;
        }
    }
}
