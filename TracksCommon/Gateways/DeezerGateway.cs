using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using TracksCommon.Entities;
using TracksCommon.Filters;
using TracksCommon.Http;
using TracksCommon.Search;

namespace TracksCommon.Gateways
{
    public class DeezerGateway : IDeezerGateway
    {
        private readonly IEnumerable<ISearch> searchs;
        private readonly IHttpPoster httpPoster;
        private readonly Dictionary<Endpoint, string> endpoints;

        public DeezerGateway(IEnumerable<ISearch> searchs, IHttpPoster httpPoster, Dictionary<Endpoint, string> endpoints)
        {
            this.searchs = searchs;
            this.httpPoster = httpPoster;
            this.endpoints = endpoints;
        }

        public string GetToken(string appId, string appSecret, string code)
        {
            var tokenUrl = string.Format(endpoints[Endpoint.AccessToken], appId, appSecret, code);

            var result = httpPoster.Request(tokenUrl, "Post");

            var api = result.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            if (api.Length == 0)
                return string.Empty;

            var token = api[0].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

            return token[1];
        }

        public DeezerUser Me(string accessToken)
        {
            var url = string.Format(endpoints[Endpoint.Me], accessToken);
            return httpPoster.RequestWithDeserialization<DeezerUser>(url, "GET");
        }
  
        public DeezerSearchItem SearchTracks(string artist, string title)
        {
            DeezerSearchItem result = DeezerSearchItem.Empty;

            foreach (var search in searchs)
            {
                var deezerSearch = httpPoster.RequestWithDeserialization<DeezerSearch>(search.GetUrl(artist, title), "GET");
                result = search.Filtering(deezerSearch, artist, title);

                if(result != null)
                    break;
            }
            
            return result;
        }

        public DeezerSearchItem GetTrack(string trackId)
        {
            var url = string.Format(endpoints[Endpoint.Track], trackId);
            var result = httpPoster.RequestWithDeserialization<DeezerSearchItem>(url, "GET");

            return result;
        }

        public Playlist GetPlaylist(string accessToken, string name)
        {
            var url = string.Format(endpoints[Endpoint.GetPlaylist], accessToken);
            var playlists = httpPoster.RequestWithDeserialization<DeezerPlaylist>(url, "GET");

            return (from item in playlists.Data where item.Title == name select item).SingleOrDefault();
        }

        public Playlist CreatePlaylist(string accessToken, string name)
        {
            var url = string.Format(endpoints[Endpoint.CreatePlaylist], accessToken, name);
            return httpPoster.RequestWithDeserialization<Playlist>(url, "POST");
        }

        public string AddToPlaylist(int id, string playlistId, string trackId, string accessToken, string searchMessage)
        {
            var url = string.Format(endpoints[Endpoint.AddToPlaylist], playlistId, accessToken, trackId);

            var result = httpPoster.Request(url, "POST");
            return string.Format("{0} - Playlist: {1}", searchMessage, result);
        }

        public IEnumerable<Genre> GetGenres(string albumId)
        {
            var url = string.Format(endpoints[Endpoint.Album], albumId);
            var response = httpPoster.Request(url, "GET");
            var jObject = JObject.Parse(response);

            if (jObject["genre_id"].ToString() == "-1")
                return new List<Genre> {new Genre {Id = "-1", Name = "NotFound"}};

            var genres = jObject["genres"]["data"].ToObject<IEnumerable<Genre>>();

            return genres ?? Enumerable.Empty<Genre>();
        }
    }
}
