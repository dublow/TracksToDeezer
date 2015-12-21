using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siriona.Library.HttpServices;

namespace TracksToDeezer.Views
{
    public class StatView : IHttpSerializer<string>
    {
        public void Serialize(IHttpResponse response, string obj)
        {
            var result = System.IO.File.ReadAllText(@"Views\stat.js");
            //result = string.Format(result, obj);
            response.ContentType = "text/html; charset=utf-8";

            using (var streamWriter = new StreamWriter(response.OutputStream))
            {
                streamWriter.Write(result);
            }
        }
    }
}
