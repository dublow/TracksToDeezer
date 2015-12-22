using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;
using TracksCommon.Filters;

namespace TracksCommon.Search
{
    public interface ISearch
    {
        string GetUrl(string artist, string title);
        DeezerSearchItem Filtering(DeezerSearch deezerSearch, string artist, string title);
    }
}
