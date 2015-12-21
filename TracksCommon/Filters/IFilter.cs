using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksCommon.Filters
{
    public interface IFilter
    {
        Search Create(DeezerSearch deezerSearch, string artist, string title, string message);
    }
}
