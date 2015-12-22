using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracksCommon.Entities
{
    public enum Endpoint
    {
        AccessToken,
        Me,
        GetPlaylist,
        CreatePlaylist,
        AddToPlaylist,
        Album,
        Track,
        FullSearch,
        ArtistSearch,
        TitleSearch
    }
}
