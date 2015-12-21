using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksCommon.Filters
{
    public class DeezerArtistFilter : IFilter
    {
        public Search Create(DeezerSearch deezerSearch, string artist, string title, string message)
        {
            var result = (from item in deezerSearch.Data
                          where String.Equals(item.Artist.Name, artist, StringComparison.CurrentCultureIgnoreCase)
                          select item).FirstOrDefault();

            if (result != null)
                result.Type = message + " Filter: ByArtist";

            return result;
        }
    }
}
