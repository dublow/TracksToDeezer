using System.Collections.Generic;
using System.Reflection.Emit;
using TracksCommon.Entities;

namespace TracksCommon.Gateways
{
    public interface IDeezerGateway
    {
        string GetToken(string appId, string appSecret, string code);
        DeezerUser Me(string accessToken);
        DeezerSearchItem SearchTracks(string artist, string title);
        DeezerSearchItem GetTrack(string trackId);
        Playlist GetPlaylist(string accessToken, string name);
        Playlist CreatePlaylist(string accessToken, string name);
        string AddToPlaylist(int id, string playlistId, string trackId, string accessToken, string searchMessage);
        IEnumerable<Genre> GetGenres(string albumId);
    }
}
