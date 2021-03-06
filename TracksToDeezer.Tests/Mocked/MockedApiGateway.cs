﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using TracksCommon.Entities;
using TracksCommon.Gateways;

namespace TracksToDeezer.Tests.Mocked
{
    public class MockedApiGateway
    {
        private readonly Mock<IApiGateway> apiGateway;
        public List<Tuple<string, string, string, string, string>> apiTable;
        public List<Tuple<string, string>> playlistTable; 

        public MockedApiGateway()
        {
            if (apiGateway == null)
            {
                apiGateway = new Mock<IApiGateway>();
                apiTable = new List<Tuple<string, string, string, string, string>>();
                playlistTable = new List<Tuple<string, string>>();
            }
                
        }

        public IApiGateway Get
        {
            get
            {
                apiGateway
                    .Setup(x => 
                        x.AddApi(
                            It.IsAny<string>(), It.IsAny<string>(), 
                            It.IsAny<string>(), It.IsAny<string>(),
                            It.IsAny<string>()))
                    .Callback<string, string, string, string, string>(AddApi);

                apiGateway
                    .Setup(x =>
                        x.RemoveApi(It.IsAny<string>()))
                    .Callback<string>(RemoveApi);

                apiGateway
                    .Setup(x =>
                        x.GetApi(It.IsAny<string>()))
                    .Returns<string>(GetApi);

                apiGateway
                    .Setup(x =>
                        x.AddPlaylist(It.IsAny<string>(), It.IsAny<string>()))
                    .Callback<string, string>(AddPlaylist);

                apiGateway
                    .Setup(x =>
                        x.GetPlaylist(It.IsAny<string>()))
                    .Returns<string>(GetPlaylist);

                return apiGateway.Object;
            }
        }

        
        private void AddApi(string apiName, string token, string userId, string firstName, string lastname)
        {
            if (apiTable.All(x => x.Item1 != apiName))
                apiTable.Add(new Tuple<string, string, string, string, string>(apiName, token, userId, firstName, lastname));
        }

        private void RemoveApi(string apiName)
        {
            if (apiTable.Any(x => x.Item1 == apiName))
            {
                var item = apiTable.SingleOrDefault(x => x.Item1 == apiName);
                apiTable.Remove(item);
            }
        }

        private Api GetApi(string apiName)
        {
            return
                apiTable.Where(x => x.Item1 == apiName)
                    .Select(x => new Api(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5))
                    .SingleOrDefault();
        }

        private void AddPlaylist(string title, string id)
        {

            if(playlistTable.All(x => x.Item1 != title))
                playlistTable.Add(new Tuple<string, string>(title, id));
        }

        private Playlist GetPlaylist(string title)
        {
            return
                playlistTable.Where(x => x.Item1 == title)
                    .Select(x => new Playlist {Title = x.Item1, Id = x.Item2})
                    .SingleOrDefault();
        }
    }
}
