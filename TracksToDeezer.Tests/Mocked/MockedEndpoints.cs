using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksToDeezer.Tests.Mocked
{
    public class MockedEndpoints
    {
        private readonly Dictionary<Endpoint, string> endpoints;  
        public MockedEndpoints()
        {
            if(endpoints == null)
                endpoints = new Dictionary<Endpoint, string>();
        }

        public Dictionary<Endpoint, string> Get
        {
            get { return endpoints; }
        }

        public void SetEndpoints(Endpoint endpoint, string path)
        {
            if (endpoints.ContainsKey(endpoint))
                endpoints[endpoint] = path;
            else
                endpoints.Add(endpoint, path);
        }
    }
}
