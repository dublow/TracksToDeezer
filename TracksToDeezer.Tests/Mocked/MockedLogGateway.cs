using System;
using System.Collections.Generic;
using Moq;
using TracksCommon.Entities;
using TracksCommon.Gateways;

namespace TracksToDeezer.Tests.Mocked
{
    public class MockedLogGateway
    {
        private readonly Mock<ILogGateway> logGateway;
        public List<Tuple<Log>> logTable;

        public MockedLogGateway()
        {
            if (logGateway == null)
            {
                logGateway = new Mock<ILogGateway>();
                logTable = new List<Tuple<Log>>();
            }
                
        }

        public ILogGateway Get
        {
            get
            {
                logGateway
                    .Setup(x =>
                        x.AddLog(It.IsAny<Log>()))
                    .Callback<Log>(AddLog);



                return logGateway.Object;
            }
        }


        private void AddLog(Log log)
        {
            logTable.Add(new Tuple<Log>(log));
        }
    }
}
