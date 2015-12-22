using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siriona.Library.EventModel;
using TracksCommon.Business;
using TracksCommon.Configurations.Deezer;
using TracksCommon.Entities;
using TracksCommon.Events;
using TracksCommon.Extensions;
using TracksCommon.Gateways;

namespace TracksToDeezer.Handlers
{
    public class SongHandler : Handle<SongAdded>.Selected
    {
        private readonly IDeezerServerConfiguration configuration;
        private readonly IApiGateway apiGateway;
        private readonly ILogGateway logGateway;
        private readonly IDeezerGateway deezerGateway;
        private readonly Dictionary<string, IRadioBusiness> radioBusiness;

        public SongHandler(IDeezerServerConfiguration configuration, IApiGateway apiGateway, IDeezerGateway deezerGateway,
            ILogGateway logGateway, Dictionary<string, IRadioBusiness> radioBusiness)
        {
            this.configuration = configuration;
            this.apiGateway = apiGateway;
            this.deezerGateway = deezerGateway;
            this.logGateway = logGateway;
            this.radioBusiness = radioBusiness;
        }

        public bool Accept(SongAdded message)
        {
            return true;
        }

        public void Handle(SongAdded message)
        {
            try
            {
                var messageUpdate = "Not Found";
                var api = apiGateway.GetApi("Deezer");
                if(api == null)
                    throw new ArgumentNullException("api", "Api not found.");

                var search = deezerGateway.SearchTracks(message.artist, message.title);
                
                var genres = Enumerable.Empty<Genre>();
                if (!search.IsEmpty)
                {
                    genres = deezerGateway.GetGenres(search.Album.Id);

                    var playlistName = string.Format(configuration.Playlist,
                        DateTime.Now.ToString("yyyyMM") + DateTime.Now.GetWeekOfMonth(), message.media);

                    var playlist = apiGateway.GetPlaylist(playlistName);

                    if (playlist == null)
                    {
                        playlist = deezerGateway.CreatePlaylist(api.token, playlistName);
                        apiGateway.AddPlaylist(playlistName, playlist.Id);
                    }
                    
                    messageUpdate = deezerGateway.AddToPlaylist(message.id, playlist.Id, search.Id, api.token, search.Type);

                    var genreString = genres.Aggregate(new StringBuilder(),
                        (builder, genre) => builder.AppendFormat("{0}-", genre.Name));

                    if(genreString.Length > 0)
                        genreString.Length--;

                    Console.WriteLine("Song Added: [{0}][{1} - {2}][{3}]", message.media, search.Artist.Name, search.Title, genreString);
                }

                radioBusiness[message.media].Update(message.id, search.Id, messageUpdate, genres);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("TracksToDeezer.Handlers {0}", ex.Message);
                logGateway.AddLog(Log.Error(errorMessage));
                Console.WriteLine(errorMessage);
            }
            
        }
    }
}
