using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TracksCommon.Entities;
using TracksCommon.Gateways;

namespace TracksToDeezer.Tests.Mocked
{
    public class MockedDeezerGateway
    {
        private readonly static Mock<IDeezerGateway> deezerGateway;
        private readonly static List<Tuple<string, string, string, string>> tokenTable;
        private readonly static List<Tuple<string, DeezerUser>> meTable;
        private static readonly List<Tuple<string, string, string, Search>> trackTable;
        private static readonly List<Tuple<string, string, Playlist>> playlistTable;
        private static readonly List<Tuple<int, string, string, string, string>> trackToPlaylistTable;
        private static readonly List<Tuple<string, IEnumerable<Genre>>> genreTable; 

        static MockedDeezerGateway()
        {
            if (deezerGateway == null)
            {
                deezerGateway = new Mock<IDeezerGateway>();
                tokenTable = new List<Tuple<string, string, string, string>>();
                meTable = new List<Tuple<string, DeezerUser>>();
                trackTable = new List<Tuple<string, string, string, Search>>();
                playlistTable = new List<Tuple<string, string, Playlist>>();
                trackToPlaylistTable = new List<Tuple<int, string, string, string, string>>();
                genreTable = new List<Tuple<string, IEnumerable<Genre>>>();
            }
        }

        public static IDeezerGateway Get
        {
            get
            {
                deezerGateway
                    .Setup(x =>
                        x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns<string, string, string>(GetToken);

                deezerGateway
                    .Setup(x =>
                        x.Me(It.IsAny<string>()))
                    .Returns<string>(Me);

                deezerGateway
                    .Setup(x =>
                        x.SearchTracks(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns<string, string>(SearchTracks);

                deezerGateway
                    .Setup(x =>
                        x.GetTrack(It.IsAny<string>()))
                    .Returns<string>(GetTrack);

                deezerGateway
                    .Setup(x =>
                        x.GetPlaylist(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns<string, string>(GetPlaylist);

                deezerGateway
                    .Setup(x =>
                        x.CreatePlaylist(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns<string, string>(CreatePlaylist);

                deezerGateway
                    .Setup(x =>
                        x.AddToPlaylist(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns<int, string, string, string, string>(AddToPlaylist);

                deezerGateway
                    .Setup(x =>
                        x.GetGenres(It.IsAny<string>()))
                    .Returns<string>(GetGenres);
                return deezerGateway.Object;
            }
        }

        public static void SetToken(string appId, string appSecret, string code, string token)
        {
            tokenTable.Add(new Tuple<string, string, string, string>(appId, appSecret, code, token));
        }

        public static void SetMe(string accessToken, DeezerUser deezerUser)
        {
            meTable.Add(new Tuple<string, DeezerUser>(accessToken, deezerUser));
        }

        public static void SetTrack(string trackId, string artist, string title, Search search)
        {
            trackTable.Add(new Tuple<string, string, string, Search>(trackId, artist, title, search));
        }

        public static void SetGenre(string albumId, IEnumerable<Genre> genres)
        {
            genreTable.Add(new Tuple<string, IEnumerable<Genre>>(albumId, genres));
        }

        private static string GetToken(string appId, string appSecret, string code)
        {
            return
                tokenTable.Where(x => x.Item1 == appId && x.Item2 == appSecret && x.Item3 == code)
                    .Select(x => x.Item4)
                    .SingleOrDefault();
        }

        private static DeezerUser Me(string accessToken)
        {
            return meTable.Where(x => x.Item1 == accessToken).Select(x => x.Item2).SingleOrDefault();
        }

        private static Search SearchTracks(string artist, string title)
        {
            return trackTable.Where(x => x.Item2 == artist && x.Item3 == title).Select(x => x.Item4).SingleOrDefault();
        }

        private static Search GetTrack(string trackId)
        {
            return trackTable.Where(x => x.Item1 == trackId).Select(x => x.Item4).SingleOrDefault();
        }

        private static Playlist GetPlaylist(string accessToken, string name)
        {
            return
                playlistTable.Where(x => x.Item1 == accessToken && x.Item2 == name)
                    .Select(x => x.Item3)
                    .SingleOrDefault();
        }

        private static Playlist CreatePlaylist(string accessToken, string name)
        {
            var playlist = new Playlist {Id = "1", Title = name};
            playlistTable.Add(new Tuple<string, string, Playlist>(accessToken, name, playlist));

            return playlist;
        }

        private static string AddToPlaylist(int id, string playlistId, string trackId, string accessToken,
            string searchMessage)
        {
            trackToPlaylistTable.Add(new Tuple<int, string, string, string, string>(id, playlistId, trackId, accessToken, searchMessage));
            return searchMessage;
        }

        private static IEnumerable<Genre> GetGenres(string albumId)
        {
            return genreTable.Where(x => x.Item1 == albumId).Select(x => x.Item2).SingleOrDefault() ?? Enumerable.Empty<Genre>();
        }
    }
}
