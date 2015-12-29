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
using TracksCommon.Http;

namespace TracksToDeezer.Tests
{
    [TestFixture]
    public class When_song_handler
    {
        [Test]
        public void When_receive_message_search_full_filter_full_success()
        {
            var mockedLogGateway = new MockedLogGateway();
            var logGateway = mockedLogGateway.Get;

            var mockedRadioBusiness = new MockedRadioBusiness();
            var radioBusiness = mockedRadioBusiness.Get;
            var dicRadioBusiness = new Dictionary<string, IRadioBusiness> { { "Fip", radioBusiness } };

            var mockedHttpPoster = new MockedHttpPoster();
            var httpPoster = mockedHttpPoster.Get;

            var mockedEndpoints = new MockedEndpoints();
            var endpoints = mockedEndpoints.Get;
            mockedEndpoints.SetEndpoints(Endpoint.CreatePlaylist, "Datas\\playlist.json");
            mockedEndpoints.SetEndpoints(Endpoint.AddToPlaylist, "Datas\\addtoplaylist.txt");
            mockedEndpoints.SetEndpoints(Endpoint.Album, "Datas\\Full\\album.json");
            mockedEndpoints.SetEndpoints(Endpoint.FullSearch, "Datas\\Full\\search.json");
            
            var mockedConf = new MockedDeezerServiceConfiguration();
            var conf = mockedConf.Get;
            mockedConf.SetAppId("160065");
            mockedConf.SetCallback("http://localhost.com");
            mockedConf.SetConnectionString("cnx");
            mockedConf.SetPlaylist("{0}-TracksFrom{1}");
            mockedConf.SetRadios(new List<string> { "Fip", "Nova", "Mfm", "Fg", "Kcsn", "Hot97", "Klosfm" });
            mockedConf.SetSecretId("3057u789dd92474e6e048cb7d7");
            mockedConf.SetServiceName("Dz");
            mockedConf.SetEndpoints(endpoints);
            
            var mockedApiGateway = new MockedApiGateway();
            var apiGateway = mockedApiGateway.Get;
            apiGateway.AddApi("Deezer", "0000000000", "4934039", "Nicolas", "Delfour");

            var filters = new List<IFilter> { new DeezerFullFilter(), new DeezerTitleFilter(), new DeezerArtistFilter() };
            var searchs = new List<ISearch>
            {
                new FullSearch(conf.Endpoints[Endpoint.FullSearch], filters)
            };

            var deezerGateway = new DeezerGateway(searchs, httpPoster, conf.Endpoints);

            var songHandler = new SongHandler(conf, apiGateway, deezerGateway, logGateway, dicRadioBusiness);
            songHandler.Handle(new SongAdded(13, "Fip", "JOHN BARRY", "THE IPCRESS FILE"));

            // Result
            var tupleBusiness = mockedRadioBusiness.radioTable.First();
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
        public void When_receive_message_search_full_filter_bytitle_success()
        {
            var mockedLogGateway = new MockedLogGateway();
            var logGateway = mockedLogGateway.Get;

            var mockedRadioBusiness = new MockedRadioBusiness();
            var radioBusiness = mockedRadioBusiness.Get;
            var dicRadioBusiness = new Dictionary<string, IRadioBusiness> { { "Fip", radioBusiness } };

            var mockedHttpPoster = new MockedHttpPoster();
            var httpPoster = mockedHttpPoster.Get;

            var mockedEndpoints = new MockedEndpoints();
            var endpoints = mockedEndpoints.Get;
            mockedEndpoints.SetEndpoints(Endpoint.CreatePlaylist, "Datas\\playlist.json");
            mockedEndpoints.SetEndpoints(Endpoint.AddToPlaylist, "Datas\\addtoplaylist.txt");
            mockedEndpoints.SetEndpoints(Endpoint.Album, "Datas\\FullByTitle\\album.json");
            mockedEndpoints.SetEndpoints(Endpoint.FullSearch, "Datas\\FullByTitle\\search.json");
            

            var mockedConf = new MockedDeezerServiceConfiguration();
            var conf = mockedConf.Get;
            mockedConf.SetAppId("160065");
            mockedConf.SetCallback("http://localhost.com");
            mockedConf.SetConnectionString("cnx");
            mockedConf.SetPlaylist("{0}-TracksFrom{1}");
            mockedConf.SetRadios(new List<string> { "Fip", "Nova", "Mfm", "Fg", "Kcsn", "Hot97", "Klosfm" });
            mockedConf.SetSecretId("3057u789dd92474e6e048cb7d7");
            mockedConf.SetServiceName("Dz");
            mockedConf.SetEndpoints(endpoints);
            
            var mockedApiGateway = new MockedApiGateway();
            var apiGateway = mockedApiGateway.Get;
            apiGateway.AddApi("Deezer", "0000000000", "4934039", "Nicolas", "Delfour");

            var filters = new List<IFilter> { new DeezerFullFilter(), new DeezerTitleFilter(), new DeezerArtistFilter() };
            var searchs = new List<ISearch>
            {
                new FullSearch(conf.Endpoints[Endpoint.FullSearch], filters)
            };

            var deezerGateway = new DeezerGateway(searchs, httpPoster, conf.Endpoints);

            var songHandler = new SongHandler(conf, apiGateway, deezerGateway, logGateway, dicRadioBusiness);
            songHandler.Handle(new SongAdded(21, "Fip", "KAWAW SIANG THONG", "KAI TOM YUM"));

            // Result
            var tupleBusiness = mockedRadioBusiness.radioTable.First();
            var genreNotFound = tupleBusiness.Item4.First(x => x.Id == "-1");

            Assert.AreEqual(tupleBusiness.Item1, 21);
            Assert.AreEqual(tupleBusiness.Item2, "16825006");
            Assert.AreEqual(tupleBusiness.Item3, "Search: Full Filter: ByTitle - Playlist: true");
            Assert.AreEqual(genreNotFound.Name, "NotFound");
        }

        [Test]
        public void When_receive_message_search_full_filter_byartist_success()
        {
            var mockedLogGateway = new MockedLogGateway();
            var logGateway = mockedLogGateway.Get;

            var mockedRadioBusiness = new MockedRadioBusiness();
            var radioBusiness = mockedRadioBusiness.Get;

            var dicRadioBusiness = new Dictionary<string, IRadioBusiness> { { "Fip", radioBusiness } };

            var mockedHttpPoster = new MockedHttpPoster();
            var httpPoster = mockedHttpPoster.Get;
            
            var mockedEndpoints = new MockedEndpoints();
            var endpoints = mockedEndpoints.Get;
            mockedEndpoints.SetEndpoints(Endpoint.CreatePlaylist, "Datas\\playlist.json");
            mockedEndpoints.SetEndpoints(Endpoint.AddToPlaylist, "Datas\\addtoplaylist.txt");
            mockedEndpoints.SetEndpoints(Endpoint.Album, "Datas\\FullByArtist\\album.json");
            mockedEndpoints.SetEndpoints(Endpoint.FullSearch, "Datas\\FullByArtist\\search.json");
            
            var mockedConf = new MockedDeezerServiceConfiguration();
            var conf = mockedConf.Get;
            mockedConf.SetAppId("160065");
            mockedConf.SetCallback("http://localhost.com");
            mockedConf.SetConnectionString("cnx");
            mockedConf.SetPlaylist("{0}-TracksFrom{1}");
            mockedConf.SetRadios(new List<string> { "Fip", "Nova", "Mfm", "Fg", "Kcsn", "Hot97", "Klosfm" });
            mockedConf.SetSecretId("3057u789dd92474e6e048cb7d7");
            mockedConf.SetServiceName("Dz");
            mockedConf.SetEndpoints(endpoints);
            
            var mockedApiGateway = new MockedApiGateway();
            var apiGateway = mockedApiGateway.Get;
            apiGateway.AddApi("Deezer", "000000000", "12345", "Nicolas", "Delfour");

            var filters = new List<IFilter> { new DeezerFullFilter(), new DeezerTitleFilter(), new DeezerArtistFilter() };
            var searchs = new List<ISearch>
            {
                new FullSearch(conf.Endpoints[Endpoint.FullSearch], filters)
            };

            var deezerGateway = new DeezerGateway(searchs, httpPoster, conf.Endpoints);

            var songHandler = new SongHandler(conf, apiGateway, deezerGateway, logGateway, dicRadioBusiness);
            songHandler.Handle(new SongAdded(32, "Fip", "LALO SCHIFRIN", "BULLITT MAIN TITLE"));

            // Result
            var tupleBusiness = mockedRadioBusiness.radioTable.First();
            var genreClassique = tupleBusiness.Item4.First(x => x.Id == "98");
            var genreJazz = tupleBusiness.Item4.First(x => x.Id == "129");
            var genreJazzInstru = tupleBusiness.Item4.First(x => x.Id == "130");
            var genreJeuVideo = tupleBusiness.Item4.First(x => x.Id == "173");
            var genreFilm = tupleBusiness.Item4.First(x => x.Id == "174");

            Assert.AreEqual(tupleBusiness.Item1, 32);
            Assert.AreEqual(tupleBusiness.Item2, "11106715");
            Assert.AreEqual(tupleBusiness.Item3, "Search: Full Filter: ByArtist - Playlist: true");
            Assert.AreEqual(genreClassique.Name, "Classique");
            Assert.AreEqual(genreJazz.Name, "Jazz");
            Assert.AreEqual(genreJazzInstru.Name, "Jazz instrumental");
            Assert.AreEqual(genreJeuVideo.Name, "Films/Jeux vidéo");
            Assert.AreEqual(genreFilm.Name, "Musiques de films");
        }

        [Test]
        public void When_receive_message_search_bytitle_filter_full_success()
        {
            var mockedLogGateway = new MockedLogGateway();
            var logGateway = mockedLogGateway.Get;

            var mockedRadioBusiness = new MockedRadioBusiness();
            var radioBusiness = mockedRadioBusiness.Get;

            var dicRadioBusiness = new Dictionary<string, IRadioBusiness> { { "Fip", radioBusiness } };

            var mockedHttpPoster = new MockedHttpPoster();
            var httpPoster = mockedHttpPoster.Get;

            var mockedEndpoints = new MockedEndpoints();
            var endpoints = mockedEndpoints.Get;
            mockedEndpoints.SetEndpoints(Endpoint.CreatePlaylist, "Datas\\playlist.json");
            mockedEndpoints.SetEndpoints(Endpoint.AddToPlaylist, "Datas\\addtoplaylist.txt");
            mockedEndpoints.SetEndpoints(Endpoint.Album, "Datas\\TitleFull\\album.json");
            mockedEndpoints.SetEndpoints(Endpoint.FullSearch, "Datas\\TitleFull\\search.json");
            mockedEndpoints.SetEndpoints(Endpoint.TitleSearch, "Datas\\TitleFull\\searchtitle.json");
            
            var mockedConf = new MockedDeezerServiceConfiguration();
            var conf = mockedConf.Get;
            mockedConf.SetAppId("160065");
            mockedConf.SetCallback("http://localhost.com");
            mockedConf.SetConnectionString("cnx");
            mockedConf.SetPlaylist("{0}-TracksFrom{1}");
            mockedConf.SetRadios(new List<string> { "Fip", "Nova", "Mfm", "Fg", "Kcsn", "Hot97", "Klosfm" });
            mockedConf.SetSecretId("3057u789dd92474e6e048cb7d7");
            mockedConf.SetServiceName("Dz");
            mockedConf.SetEndpoints(endpoints);
            
            var mockedApiGateway = new MockedApiGateway();
            var apiGateway = mockedApiGateway.Get;
            apiGateway.AddApi("Deezer", "0000000000", "4934039", "Nicolas", "Delfour");

            var filters = new List<IFilter> { new DeezerFullFilter(), new DeezerTitleFilter(), new DeezerArtistFilter() };
            var searchs = new List<ISearch>
            {
                new FullSearch(conf.Endpoints[Endpoint.FullSearch], filters),
                new TitleSearch(conf.Endpoints[Endpoint.TitleSearch], filters)
            };

            var deezerGateway = new DeezerGateway(searchs, httpPoster, conf.Endpoints);

            var songHandler = new SongHandler(conf, apiGateway, deezerGateway, logGateway, dicRadioBusiness);
            songHandler.Handle(new SongAdded(7420, "Fip", "ANGUS & JULIA STONE", "FROM THE STALLS"));

            // Result
            var tupleBusiness = mockedRadioBusiness.radioTable.First();
            var genreAlt = tupleBusiness.Item4.First(x => x.Id == "85");
            var genrePopInde = tupleBusiness.Item4.First(x => x.Id == "86");
            var genrePop = tupleBusiness.Item4.First(x => x.Id == "132");

            Assert.AreEqual(tupleBusiness.Item1, 7420);
            Assert.AreEqual(tupleBusiness.Item2, "80164472");
            Assert.AreEqual(tupleBusiness.Item3, "Search: ByTitle Filter: Full - Playlist: true");
            Assert.AreEqual(genreAlt.Name, "Alternative");
            Assert.AreEqual(genrePopInde.Name, "Pop Indé");
            Assert.AreEqual(genrePop.Name, "Pop");
        }

        [Test]
        public void When_receive_message_search_bytitle_filter_bytitle_success()
        {
            var mockedLogGateway = new MockedLogGateway();
            var logGateway = mockedLogGateway.Get;

            var mockedRadioBusiness = new MockedRadioBusiness();
            var radioBusiness = mockedRadioBusiness.Get;

            var dicRadioBusiness = new Dictionary<string, IRadioBusiness> { { "Fip", radioBusiness } };

            var mockedHttpPoster = new MockedHttpPoster();
            var httpPoster = mockedHttpPoster.Get;

            var mockedEndpoints = new MockedEndpoints();
            var endpoints = mockedEndpoints.Get;
            mockedEndpoints.SetEndpoints(Endpoint.CreatePlaylist, "Datas\\playlist.json");
            mockedEndpoints.SetEndpoints(Endpoint.AddToPlaylist, "Datas\\addtoplaylist.txt");
            mockedEndpoints.SetEndpoints(Endpoint.Album, "Datas\\TitleByTitle\\album.json");
            mockedEndpoints.SetEndpoints(Endpoint.FullSearch, "Datas\\TitleByTitle\\search.json");
            mockedEndpoints.SetEndpoints(Endpoint.TitleSearch, "Datas\\TitleByTitle\\searchtitle.json");

            var mockedConf = new MockedDeezerServiceConfiguration();
            var conf = mockedConf.Get;
            mockedConf.SetAppId("160065");
            mockedConf.SetCallback("http://localhost.com");
            mockedConf.SetConnectionString("cnx");
            mockedConf.SetPlaylist("{0}-TracksFrom{1}");
            mockedConf.SetRadios(new List<string> { "Fip", "Nova", "Mfm", "Fg", "Kcsn", "Hot97", "Klosfm" });
            mockedConf.SetSecretId("3057u789dd92474e6e048cb7d7");
            mockedConf.SetServiceName("Dz");
            mockedConf.SetEndpoints(endpoints);

            var mockedApiGateway = new MockedApiGateway();
            var apiGateway = mockedApiGateway.Get;
            apiGateway.AddApi("Deezer", "0000000000", "4934039", "Nicolas", "Delfour");

            var filters = new List<IFilter> { new DeezerFullFilter(), new DeezerTitleFilter(), new DeezerArtistFilter() };
            var searchs = new List<ISearch>
            {
                new FullSearch(conf.Endpoints[Endpoint.FullSearch], filters),
                new TitleSearch(conf.Endpoints[Endpoint.TitleSearch], filters)
            };

            var deezerGateway = new DeezerGateway(searchs, httpPoster, conf.Endpoints);

            var songHandler = new SongHandler(conf, apiGateway, deezerGateway, logGateway, dicRadioBusiness);
            songHandler.Handle(new SongAdded(43, "Fip", "DE LA SOUL", "EYE KNOW"));

            // Result
            var tupleBusiness = mockedRadioBusiness.radioTable.First();
            var genreAlt = tupleBusiness.Item4.First(x => x.Id == "85");
            var genreElectro = tupleBusiness.Item4.First(x => x.Id == "106");

            Assert.AreEqual(tupleBusiness.Item1, 43);
            Assert.AreEqual(tupleBusiness.Item2, "84731153");
            Assert.AreEqual(tupleBusiness.Item3, "Search: ByTitle Filter: ByTitle - Playlist: true");
            Assert.AreEqual(genreAlt.Name, "Alternative");
            Assert.AreEqual(genreElectro.Name, "Electro");
        }
    }
}
