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

        public MockedHttpPoster()
        {
            if (httpPoster == null)
            {
                httpPoster = new Mock<IHttpPoster>();
            }
        }

        public IHttpPoster Get
        {
            get
            {
                httpPoster
                    .Setup(x => x.Get(It.IsAny<string>()))
                    .Returns<string>(Request);

                httpPoster
                    .Setup(x => x.Post(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .Returns<string, Dictionary<string, string>>(Request);

                httpPoster
                    .Setup(x => x.Get<DeezerUser>(It.IsAny<string>()))
                    .Returns<string>(RequestWithDeserialization<DeezerUser>);

                httpPoster
                    .Setup(x => x.Get<DeezerSearchItem>(It.IsAny<string>()))
                    .Returns<string>(RequestWithDeserialization<DeezerSearchItem>);

                httpPoster
                    .Setup(x => x.Get<Playlist>(It.IsAny<string>()))
                    .Returns<string>(RequestWithDeserialization<Playlist>);

                httpPoster
                    .Setup(x => x.Post<Playlist>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .Returns<string, Dictionary<string, string>>(RequestWithDeserialization<Playlist>);

                httpPoster
                    .Setup(x => x.Get<DeezerSearch>(It.IsAny<string>()))
                    .Returns<string>(RequestWithDeserialization<DeezerSearch>);

                httpPoster
                    .Setup(x => x.Get<DeezerPlaylist>(It.IsAny<string>()))
                    .Returns<string>(RequestWithDeserialization<DeezerPlaylist>);

                return httpPoster.Object;
            }
        }

        private string Request(string url)
        {
            return GetFile(url);
        }

        private string Request(string url, Dictionary<string, string> data)
        {
            return GetFile(url);
        }

        private T RequestWithDeserialization<T>(string url)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(GetFile(url));
        }

        private T RequestWithDeserialization<T>(string url, Dictionary<string, string> data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(GetFile(url));
        }

        private string GetFile(string path)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string location = Path.Combine(executableLocation, path);
            return File.ReadAllText(location);
            //return File.ReadAllText(path);
        }
    }
}
