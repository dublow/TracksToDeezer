using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracksCommon.Http
{
    public interface IHttpPoster
    {
        T RequestWithDeserialization<T>(string url, string method);
        string Request(string url, string method);
    }
}
