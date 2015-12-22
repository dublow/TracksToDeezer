using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TracksCommon.Business;
using TracksCommon.Entities;
using TracksCommon.Events;
using TracksToDeezer.Handlers;
using TracksToDeezer.Tests.Mocked;
using TracksCommon.Gateways;
using TracksCommon.Search;
using TracksCommon.Filters;

namespace TracksToDeezer.Tests
{
    [TestFixture]
    public class When_song_handler
    {
        [Test]
        public void When_receive_message_full_success()
        {
            // Init
            var conf = MockedDeezerServiceConfiguration.Get;
            var apiGateway = MockedApiGateway.Get;
            var logGateway = MockedLogGateway.Get;
            var radioBusiness = new Dictionary<string, IRadioBusiness> {{"Fip", MockedRadioBusiness.Get } };
            var httpPoster = MockedHttpPoster.Get;
            var endpoints = new Dictionary<Endpoint, string>
            {
                { Endpoint.AccessToken, "" },
                { Endpoint.Me, "" },
                { Endpoint.GetPlaylist, "" },
                { Endpoint.CreatePlaylist, "Datas\\playlist.json" },
                { Endpoint.AddToPlaylist, "Datas\\addtoplaylist.txt" },
                { Endpoint.Album, "Datas\\album.json" },
                { Endpoint.Track, "" },
                { Endpoint.FullSearch, "Datas\\fullsearch.json" },
                { Endpoint.ArtistSearch, "" },
                { Endpoint.TitleSearch, "" },
            };

            // Setup
            apiGateway.AddApi("Deezer", "000000000", "12345", "Nicolas", "Delfour");

            MockedDeezerServiceConfiguration.SetAppId("160065");
            MockedDeezerServiceConfiguration.SetCallback("http://localhost.com");
            MockedDeezerServiceConfiguration.SetConnectionString("cnx");
            MockedDeezerServiceConfiguration.SetPlaylist("{0}-TracksFrom{1}");
            MockedDeezerServiceConfiguration.SetRadios(new List<string> { "Fip", "Nova", "Mfm", "Fg", "Kcsn", "Hot97", "Klosfm" });
            MockedDeezerServiceConfiguration.SetSecretId("3057u789dd92474e6e048cb7d7");
            MockedDeezerServiceConfiguration.SetServiceName("Dz");
            MockedDeezerServiceConfiguration.SetEndpoints(endpoints);

            var filters = new List<IFilter> { new DeezerFullFilter(), new DeezerTitleFilter(), new DeezerArtistFilter() };
            var searchs = new List<ISearch>
            {
                new FullSearch(conf.Endpoints[Endpoint.FullSearch], filters),
                new TitleSearch(conf.Endpoints[Endpoint.TitleSearch], filters),
                new ArtistSearch(conf.Endpoints[Endpoint.ArtistSearch], filters)
            };

            var deezerGateway = new DeezerGateway(searchs, httpPoster, conf.Endpoints);

            var songHandler = new SongHandler(conf, apiGateway, deezerGateway, logGateway, radioBusiness);
            songHandler.Handle(new SongAdded(13, "Fip", "JOHN BARRY", "THE IPCRESS FILE"));

            // Result
            var tupleBusiness = MockedRadioBusiness.radioTable.First();
            var genrePop = tupleBusiness.Item4.First(x=>x.Id == "132");
            var genreJeuVideo = tupleBusiness.Item4.First(x => x.Id == "173");
            var genreFilm = tupleBusiness.Item4.First(x => x.Id == "174");

            Assert.AreEqual(tupleBusiness.Item1, 13);
            Assert.AreEqual(tupleBusiness.Item2, "1037339");
            Assert.AreEqual(tupleBusiness.Item3, "Search: Full Filter: Full - Playlist: true");
            Assert.AreEqual(genrePop.Name, "Pop");
            Assert.AreEqual(genreJeuVideo.Name, "Films/Jeux vidéo");
            Assert.AreEqual(genreFilm.Name, "Musiques de films");
        }

        [Test]
        public void When_receive_message_api_not_found()
        {
            // Init
            var conf = MockedDeezerServiceConfiguration.Get;
            var apiGateway = MockedApiGateway.Get;
            var logGateway = MockedLogGateway.Get;
            var deezerGateway = MockedDeezerGateway.Get;
            var radioBusiness = MockedRadioBusiness.Get;
            var dicBusiness = new Dictionary<string, IRadioBusiness> { { "Fip", radioBusiness } };

            // Setup
            MockedDeezerServiceConfiguration.SetAppId("160065");
            MockedDeezerServiceConfiguration.SetCallback("http://localhost.com");
            MockedDeezerServiceConfiguration.SetConnectionString("cnx");
            MockedDeezerServiceConfiguration.SetPlaylist("{0}-TracksFrom{1}");
            MockedDeezerServiceConfiguration.SetRadios(new List<string> { "Fip", "Nova", "Mfm", "Fg", "Kcsn", "Hot97", "Klosfm" });
            MockedDeezerServiceConfiguration.SetSecretId("3057u789dd92474e6e048cb7d7");
            MockedDeezerServiceConfiguration.SetServiceName("Dz");

            var album = new Album { Id = "14678" };
            var artist = new Artist { Name = "JOHN BARRY" };
            var search = new DeezerSearchItem
            {
                Album = album,
                Artist = artist,
                Id = "1037339",
                Title = "THE IPCRESS FILE",
                Type = "Search: Full Filter: Full - Playlist: true"
            };

            MockedDeezerGateway.SetToken(conf.AppId, conf.SecretId, "123", "000000000");
            MockedDeezerGateway.SetGenre("14678", new List<Genre> { new Genre { Id = "12", Name = "Pop" } });
            MockedDeezerGateway.SetMe("000000000", new DeezerUser { Firstname = "Nicolas", Id = "12345", Lastname = "Delfour" });
            MockedDeezerGateway.SetTrack("13", "JOHN BARRY", "THE IPCRESS FILE", search);

            var songHandler = new SongHandler(conf, apiGateway, deezerGateway, logGateway, dicBusiness);
            songHandler.Handle(new SongAdded(13, "Fip", "JOHN BARRY", "THE IPCRESS FILE"));

            var log = MockedLogGateway.logTable.First();
            Assert.AreEqual(log.Item1.type, "Error");
            Assert.AreEqual(log.Item1.message, "TracksToDeezer.Handlers Api not found.\r\nNom du paramètre : api");
        }


    }
}
