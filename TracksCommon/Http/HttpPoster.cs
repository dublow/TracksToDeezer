using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TracksCommon.Entities;
using System.Text;

namespace TracksCommon.Http
{
    public class HttpPoster : IHttpPoster
    {
        public T Get<T>(string url) where T : IDeserializer
        {
            string result = Request(url, "GET", null);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
        }

        public T Post<T>(string url, Dictionary<string, string> datas) where T : IDeserializer
        {
            string result = Request(url, "POST", datas);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
        }

        public string Get(string url)
        {
            return Request(url, "GET", null);
        }

        public string Post(string url, Dictionary<string, string> datas)
        {
            return Request(url, "POST", datas);
        }

        public string Request(string url, string method, Dictionary<string, string> datas)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.Method = method;
            if (method == "POST")
            {
                var streamData = GetFormDatas(datas);
                if(streamData.Any())
                {
                    webRequest.ContentLength = streamData.Length;
                    using (var stream = webRequest.GetRequestStream())
                    {
                        stream.Write(streamData, 0, streamData.Length);
                    }
                }
                
            }

            using (var webResponse = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        private byte[] GetFormDatas(Dictionary<string, string> datas)
        {
            if (datas == null)
                return Enumerable.Empty<byte>().ToArray();

            var dataForm = datas.Aggregate(new StringBuilder(), (cur, next) => { return cur.AppendFormat("{0}={1}&", next.Key, next.Value); });
            dataForm.Length = dataForm.Length - 1;
            return Encoding.UTF8.GetBytes(dataForm.ToString());
        }
    }
}
