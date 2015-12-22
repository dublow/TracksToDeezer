using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksCommon.Filters
{
    public class DeezerFullFilter : IFilter
    {
        public DeezerSearchItem Create(DeezerSearch deezerSearch, string artist, string title, string message)
        {
            var result = (from item in deezerSearch.Data
                          where String.Equals(item.Title, title, StringComparison.CurrentCultureIgnoreCase)
                            && String.Equals(item.Artist.Name, artist, StringComparison.CurrentCultureIgnoreCase)
                          select item).FirstOrDefault();

            if(result != null)
                result.Type = message + " Filter: Full";

            return result;
        }
    }
}
