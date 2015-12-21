using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using Newtonsoft.Json.Linq;
using TracksCommon.Entities;
using TracksCommon.Extensions;
using TracksCommon.Filters;

namespace TracksCommon.Gateways
{
    public class DeezerGateway : IDeezerGateway
    {
        private readonly IEnumerable<TrackManager> trackManagers;

        public DeezerGateway(IEnumerable<TrackManager> trackManagers)
        {
            this.trackManagers = trackManagers;
        }

        public string GetToken(string appId, string appSecret, string code)
        {
            var tokenUrl = string.Format("https://connect.deezer.com/oauth/access_token.php?app_id={0}&secret={1}&code={2}",
                    appId, appSecret, code);

            var webRequest = WebRequest.Create(tokenUrl);
            webRequest.Method = "POST";

            using (var webResponse = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var api = result.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

                    if (api.Length == 0)
                        return string.Empty;

                    var token = api[0].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                    return token[1];
                }
            }
        }

        public DeezerUser Me(string accessToken)
        {
            var url = string.Format("http://api.deezer.com/user/me?access_token={0}", accessToken);
            return RequestWithDeserialization<DeezerUser>(url, "GET");
        }

        //public Search SearchTracks(string artist, string title)
        //{
        //    var message = "";

        //    var url = string.Format("https://api.deezer.com/search?strict=on&q=artist:\"{0}\" track:\"{1}\"", artist, title);

        //    var response = RequestWithDeserialization<DeezerSearch>(url, "GET");
        //    if (response == null || !response.Data.Any())
        //    {
        //        url = string.Format("https://api.deezer.com/search?strict=on&q=track:\"{0}\"", title);
        //        response = RequestWithDeserialization<DeezerSearch>(url, "GET");

        //        if (response == null || !response.Data.Any())
        //        {
        //            url = string.Format("https://api.deezer.com/search?strict=on&q=artist:\"{0}\"", artist);
        //            response = RequestWithDeserialization<DeezerSearch>(url, "GET");
        //            message = "Search: ByArtist ";
        //        }
        //        else
        //        {
        //            message = "Search: ByTitle ";
        //        }
        //    }
        //    else
        //    {
        //        message = "Search: Full ";
        //    }

        //    var result = (from item in response.Data
        //                  where String.Equals(item.Title, title, StringComparison.CurrentCultureIgnoreCase)
        //                    && String.Equals(item.Artist.Name, artist, StringComparison.CurrentCultureIgnoreCase)
        //                  select item).FirstOrDefault();

        //    if (result != null)
        //    {
        //        result.Type = message + "Filter: Full";
        //        return result;
        //    }

        //    result = (from item in response.Data
        //              where String.Equals(item.Title, title, StringComparison.CurrentCultureIgnoreCase)
        //              select item).FirstOrDefault();

        //    if (result != null)
        //    {
        //        result.Type = message + "Filter: ByTitle";
        //        return result;
        //    }

        //    result = (from item in response.Data
        //              where String.Equals(item.Artist.Name, artist, StringComparison.CurrentCultureIgnoreCase)
        //              select item).FirstOrDefault();

        //    if (result != null)
        //    {
        //        result.Type = message + "Filter: ByArtist";
        //        return result;
        //    }

        //    return result;
        //}

        public Search SearchTracks(string artist, string title)
        {
            Search result = null;

            foreach (var trackManager in trackManagers)
            {
                var url = trackManager.GetUrl(artist, title);

                var deezerSearch = RequestWithDeserialization<DeezerSearch>(url, "GET");
                result = trackManager.Filtering(deezerSearch, artist, title);

                if(result != null)
                    break;
            }
            
            return result;
        }

        public Search GetTrack(string trackId)
        {
            var url = string.Format("https://api.deezer.com/track/{0}", trackId);
            var result = RequestWithDeserialization<Search>(url, "GET");

            return result;
        }

        public Playlist GetPlaylist(string accessToken, string name)
        {
            var url = string.Format("http://api.deezer.com/user/me/playlists?access_token={0}", accessToken);
            var playlists = RequestWithDeserialization<DeezerPlaylist>(url, "GET");

            return (from item in playlists.Data where item.Title == name select item).SingleOrDefault();
        }

        public Playlist CreatePlaylist(string accessToken, string name)
        {
            var url = string.Format("http://api.deezer.com/user/me/playlists?access_token={0}&title={1}", accessToken, name);
            return RequestWithDeserialization<Playlist>(url, "POST");
        }

        public string AddToPlaylist(int id, string playlistId, string trackId, string accessToken, string searchMessage)
        {
            var url = string.Format("https://api.deezer.com/playlist/{0}/tracks?request_method=POST&access_token={1}&songs={2}", playlistId, accessToken, trackId);

            var webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";

            using (var webResponse = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return string.Format("{0} - Playlist: {1}", searchMessage, result);
                }
            }
        }

        public IEnumerable<Genre> GetGenres(string albumId)
        {
            var url = string.Format("https://api.deezer.com/album/{0}", albumId);
            var response = Request(url, "GET");
            var jObject = JObject.Parse(response);

            if (jObject["genre_id"].ToString() == "-1")
                return new List<Genre> {new Genre {Id = "-1", Name = "NotFound"}};

            var genres = jObject["genres"]["data"].ToObject<IEnumerable<Genre>>();

            return genres;
        }

        private T RequestWithDeserialization<T>(string url, string method) where T : class
        {
            string result = Request(url, method);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
        }

        private string Request(string url, string method)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.Method = method;

            using (var webResponse = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
