﻿using System.IO;
using System.Net;

namespace TracksCommon.Http
{
    public class HttpPoster : IHttpPoster
    {
        public T RequestWithDeserialization<T>(string url, string method)
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
    }
}