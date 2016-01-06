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
        public T RequestWithDeserialization<T>(string url, string method) where T : IDeserializer
        {
            string result = Request(url, method);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
        }

        public string Request(string url, string method)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.Method = method;

            using (var webResponse = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public string Post(string url, Dictionary<string, string> datas)
        {
            var dataForm = datas.Aggregate(new StringBuilder(), (cur, next) => { return cur.AppendFormat("{0}={1}&", next.Key, next.Value); });
            dataForm.Length = dataForm.Length - 1;
            var streamData = Encoding.UTF8.GetBytes(dataForm.ToString());

            var webRequest = WebRequest.Create(url);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = streamData.Length;

            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(streamData, 0, streamData.Length);
            }

            using (var webResponse = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            
        }
    }
}
