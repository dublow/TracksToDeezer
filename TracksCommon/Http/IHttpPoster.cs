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
        T Get<T>(string url) where T: IDeserializer;
        string Get(string url);
        T Post<T>(string url, Dictionary<string, string> datas) where T : IDeserializer;
        string Post(string url, Dictionary<string, string> datas);
    }
}
