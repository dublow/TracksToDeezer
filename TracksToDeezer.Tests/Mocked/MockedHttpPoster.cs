using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;
using TracksCommon.Http;

namespace TracksToDeezer.Tests.Mocked
{
    public class MockedHttpPoster
    {
        private static Mock<IHttpPoster> httpPoster;

        static MockedHttpPoster()
        {
            if (httpPoster == null)
            {
                httpPoster = new Mock<IHttpPoster>();
            }
        }

        public static IHttpPoster Get
        {
            get
            {
                httpPoster
                    .Setup(x => x.Request(It.IsAny<string>(), It.IsIn("GET", "POST")))
                    .Returns<string, string>(Request);

                httpPoster
                    .Setup(x => x.RequestWithDeserialization<DeezerSearch>(It.IsAny<string>(), It.IsIn("GET", "POST")))
                    .Returns<string, string>(RequestWithDeserialization<DeezerSearch>);

                httpPoster
                    .Setup(x => x.RequestWithDeserialization<DeezerSearchItem>(It.IsAny<string>(), It.IsIn("GET", "POST")))
                    .Returns<string, string>(RequestWithDeserialization<DeezerSearchItem>);

                httpPoster
                    .Setup(x => x.RequestWithDeserialization<DeezerUser>(It.IsAny<string>(), It.IsIn("GET", "POST")))
                    .Returns<string, string>(RequestWithDeserialization<DeezerUser>);

                httpPoster
                    .Setup(x => x.RequestWithDeserialization<DeezerPlaylist>(It.IsAny<string>(), It.IsIn("GET", "POST")))
                    .Returns<string, string>(RequestWithDeserialization<DeezerPlaylist>);

                httpPoster
                    .Setup(x => x.RequestWithDeserialization<Playlist>(It.IsAny<string>(), It.IsIn("GET", "POST")))
                    .Returns<string, string>(RequestWithDeserialization<Playlist>);

                return httpPoster.Object;
            }
        }

        private static string Request(string url, string method)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string location = Path.Combine(executableLocation, url);
            return File.ReadAllText(location);
        }

        private static T RequestWithDeserialization<T>(string url, string method)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string location = Path.Combine(executableLocation, url);
            var value = File.ReadAllText(location);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }
    }
}
