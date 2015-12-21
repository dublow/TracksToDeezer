using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksCommon.Filters
{
    public class DeezerTitleFilter : IFilter
    {
        public Search Create(DeezerSearch deezerSearch, string artist, string title, string message)
        {
            var result = (from item in deezerSearch.Data
                          where String.Equals(item.Title, title, StringComparison.CurrentCultureIgnoreCase)
                          select item).FirstOrDefault();

            if (result != null)
                result.Type = message + " Filter: ByTitle";

            return result;
        }
    }
}
