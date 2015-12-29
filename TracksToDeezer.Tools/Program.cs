using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Siriona.Library.ServiceBus;
using TracksCommon.Configurations.Deezer;
using TracksCommon.Entities;
using TracksCommon.Events;
using TracksCommon.Filters;
using TracksCommon.Gateways;
using TracksCommon.Http;
using TracksCommon.Providers;
using TracksCommon.Search;

namespace TracksToDeezer.Tools
{
    class Program
    {
        static void Main(string[] args)
        {

            var conf = new DeezerConfiguration();
            var endpoints = conf.Endpoints;
            var sql = new SqlConnectionProvider(conf.ConnectionString);
            var radios = LoadRadios(sql, conf.Radios);
            var radio = radios["Fip"];

            var filters = new List<IFilter> { new DeezerFullFilter(), new DeezerArtistFilter(), new DeezerArtistFilter() };
            var searchs = new List<ISearch>
            {
                new FullSearch(endpoints[Endpoint.FullSearch], filters),
                new TitleSearch(endpoints[Endpoint.FullSearch], filters),
                new ArtistSearch(endpoints[Endpoint.FullSearch], filters)
            };

            // Get Trackid when trackid is null in DB
            if (args.Any(x => x == "u"))
            {
                var httpPoster = new HttpPoster();
                var songs = radio.GetAllSongIsNull();
                UpdateTitle(radio, searchs, songs, httpPoster, endpoints);
                Console.ReadLine();
            }
           
            // Get Song from db when is not sent to deezer
            if (args.Any(x => x == "s"))
            {
                var songs = radio.GetAllSongIsUnpublished();
                PublishToDeezer(songs, radio.Name);
            }

            // Get Song from db for getting genre
            if (args.Any(x => x == "g"))
            {
                var httpPoster = new HttpPoster();
                var songs = radio.GetAllHasNoGenre();
                UpdateGenre(radio, searchs, songs, httpPoster, endpoints);
                Console.ReadLine();
            }
        }

        static Dictionary<string, IRadioGateway> LoadRadios(ISqlConnectionProvider sql, List<string> radios)
        {
            return radios
                    .ToDictionary<string, string, IRadioGateway>(radio => radio, radio => new RadioGateway(sql, radio));
        }

        async static void UpdateTitle(IRadioGateway radio, IEnumerable<ISearch> searchs, IEnumerable<SongFromDb> songs, IHttpPoster httpPoster, Dictionary<Endpoint, string> endpoints)
        {
            var deezer = new DeezerGateway(searchs, httpPoster, endpoints);
            foreach (var songFromDb in songs)
            {
                var result = deezer.SearchTracks(songFromDb.artiste, songFromDb.title);
                var trackId = result != null ? result.Id : "-1";
                radio.UpdateTrackId(songFromDb.id, trackId);
                Console.WriteLine("SONG: {0} - {1}", songFromDb.id, trackId);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            
        }

        async static void UpdateGenre(IRadioGateway radio, IEnumerable<ISearch> searchs, IEnumerable<SongFromDb> songs, IHttpPoster httpPoster, Dictionary<Endpoint, string> endpoints)
        {
            var deezer = new DeezerGateway(searchs, httpPoster, endpoints);
            foreach (var songFromDb in songs)
            {
                var track = deezer.GetTrack(songFromDb.trackId);
                var genres = deezer.GetGenres(track.Album.Id);

                foreach (var genre in genres)
                {
                    if(genre.Id != "-1")
                        radio.AddGenre(genre);

                    radio.AddGenreToTrack(songFromDb.id, int.Parse(genre.Id));

                    Console.WriteLine("Genre: {0} - {1}", genre.Id, genre.Name);
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }

        }

        static void PublishToDeezer(IEnumerable<SongFromDb> songs, string media)
        {
            var clientBus = ClientBus.Named("TracksFromDeezer.Tools").Reliable(false).CreateBus();
            foreach (var songFromDb in songs)
            {
                clientBus.Publish(new SongAdded(songFromDb.id, media, songFromDb.artiste, songFromDb.title));
                Console.WriteLine("SONG PUBLISHED : {0} - {1} ({2})", songFromDb.id, songFromDb.artiste, songFromDb.title);
            }

            Console.ReadLine();
        }
    }
}
