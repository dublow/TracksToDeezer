﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TracksCommon.Business;
using TracksCommon.Entities;

namespace TracksToDeezer.Tests.Mocked
{
    public class MockedRadioBusiness
    {
        private readonly Mock<IRadioBusiness> radioBusiness;
        public readonly List<Tuple<int, string, string, IEnumerable<Genre>>> radioTable;
 
        public MockedRadioBusiness()
        {
            if (radioBusiness == null)
            {
                radioBusiness = new Mock<IRadioBusiness>();
                radioTable = new List<Tuple<int, string, string, IEnumerable<Genre>>>();
            }
        }

        public IRadioBusiness Get
        {
            get
            {
                radioBusiness
                    .Setup(x =>
                        x.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<Genre>>()))
                    .Callback<int, string, string, IEnumerable<Genre>>(Update);

                return radioBusiness.Object;
            }
        }

        private void Update(int id, string trackId, string message, IEnumerable<Genre> genres)
        {
            radioTable.Add(new Tuple<int, string, string, IEnumerable<Genre>>(id, trackId, message, genres));
        }
    }
}
