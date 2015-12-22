using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksCommon.Http
{
    public interface IHttpPoster
    {
        T RequestWithDeserialization<T>(string url, string method) where T: IDeserializer;
        string Request(string url, string method);
    }
}
