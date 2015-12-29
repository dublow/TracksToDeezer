using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TracksCommon.Configurations.Deezer;
using TracksCommon.Entities;

namespace TracksToDeezer.Tests.Mocked
{
    public class MockedDeezerServiceConfiguration
    {
        private static Mock<IDeezerServerConfiguration> conf;

        public MockedDeezerServiceConfiguration()
        {
            if(conf == null)
                conf = new Mock<IDeezerServerConfiguration>();
        }

        public IDeezerServerConfiguration Get
        {
            get
            {
                conf = new Mock<IDeezerServerConfiguration>();
                conf.Setup(x => x.AppId).Returns("");
                conf.Setup(x => x.Callback).Returns("");
                conf.Setup(x => x.ConnectionString).Returns("");
                conf.Setup(x => x.Playlist).Returns("");
                conf.Setup(x => x.SecretId).Returns("");
                conf.Setup(x => x.ServiceName).Returns("");
                conf.Setup(x => x.Radios).Returns(new List<string>());
                conf.Setup(x => x.Endpoints).Returns(new Dictionary<Endpoint, string>());

                return conf.Object;
            }
        }

        public void SetAppId(string appId)
        {
            conf.Setup(x => x.AppId).Returns(appId);
        }

        public void SetCallback(string callback)
        {
            conf.Setup(x => x.Callback).Returns(callback);
        }

        public void SetConnectionString(string connectionString)
        {
            conf.Setup(x => x.ConnectionString).Returns(connectionString);
        }

        public void SetPlaylist(string playlist)
        {
            conf.Setup(x => x.Playlist).Returns(playlist);
        }

        public void SetSecretId(string secretId)
        {
            conf.Setup(x => x.SecretId).Returns(secretId);
        }

        public void SetServiceName(string serviceName)
        {
            conf.Setup(x => x.ServiceName).Returns(serviceName);
        }

        public void SetRadios(List<string> radios)
        {
            conf.Setup(x => x.Radios).Returns(radios);
        }

        public void SetEndpoints(Dictionary<Endpoint, string> endpoints)
        {
            conf.Setup(x => x.Endpoints).Returns(endpoints);
        }
    }
}
