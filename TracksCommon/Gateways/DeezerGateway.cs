using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using TracksCommon.Entities;
using TracksCommon.Filters;
using TracksCommon.Http;

namespace TracksCommon.Gateways
{
    public class DeezerGateway : IDeezerGateway
    {
        private readonly IEnumerable<TrackManager> trackManagers;
        private readonly IHttpPoster httpPoster;

        public DeezerGateway(IEnumerable<TrackManager> trackManagers, IHttpPoster httpPoster)
        {
            this.trackManagers = trackManagers;
            this.httpPoster = httpPoster;
        }

        public string GetToken(string appId, string appSecret, string code)
        {
            var tokenUrl = string.Format("https://connect.deezer.com/oauth/access_token.php?app_id={0}&secret={1}&code={2}",
                    appId, appSecret, code);

            var result = httpPoster.Request(tokenUrl, "Post");

            var api = result.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            if (api.Length == 0)
                return string.Empty;

            var token = api[0].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

            return token[1];
        }

        public DeezerUser Me(string accessToken)
        {
            var url = string.Format("http://api.deezer.com/user/me?access_token={0}", accessToken);
            return httpPoster.RequestWithDeserialization<DeezerUser>(url, "GET");
        }
  
        public Search SearchTracks(string artist, string title)
        {
            Search result = Search.Empty;

            foreach (var trackManager in trackManagers)
            {
                var url = trackManager.GetUrl(artist, title);

                var deezerSearch = httpPoster.RequestWithDeserialization<DeezerSearch>(url, "GET");
                result = trackManager.Filtering(deezerSearch, artist, title);

                if(result != null)
                    break;
            }
            
            return result;
        }

        public Search GetTrack(string trackId)
        {
            var url = string.Format("https://api.deezer.com/track/{0}", trackId);
            var result = httpPoster.RequestWithDeserialization<Search>(url, "GET");

            return result;
        }

        public Playlist GetPlaylist(string accessToken, string name)
        {
            var url = string.Format("http://api.deezer.com/user/me/playlists?access_token={0}", accessToken);
            var playlists = httpPoster.RequestWithDeserialization<DeezerPlaylist>(url, "GET");

            return (from item in playlists.Data where item.Title == name select item).SingleOrDefault();
        }

        public Playlist CreatePlaylist(string accessToken, string name)
        {
            var url = string.Format("http://api.deezer.com/user/me/playlists?access_token={0}&title={1}", accessToken, name);
            return httpPoster.RequestWithDeserialization<Playlist>(url, "POST");
        }

        public string AddToPlaylist(int id, string playlistId, string trackId, string accessToken, string searchMessage)
        {
            var url = string.Format("https://api.deezer.com/playlist/{0}/tracks?request_method=POST&access_token={1}&songs={2}", playlistId, accessToken, trackId);

            var result = httpPoster.Request(url, "POST");
            return string.Format("{0} - Playlist: {1}", searchMessage, result);
        }

        public IEnumerable<Genre> GetGenres(string albumId)
        {
            var url = string.Format("https://api.deezer.com/album/{0}", albumId);
            var response = httpPoster.Request(url, "GET");
            var jObject = JObject.Parse(response);

            if (jObject["genre_id"].ToString() == "-1")
                return new List<Genre> {new Genre {Id = "-1", Name = "NotFound"}};

            var genres = jObject["genres"]["data"].ToObject<IEnumerable<Genre>>();

            return genres ?? Enumerable.Empty<Genre>();
        }
    }
}
