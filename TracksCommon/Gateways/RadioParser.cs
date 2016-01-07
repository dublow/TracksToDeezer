using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TracksCommon.Configurations.Radio;
using TracksCommon.Entities;
using TracksCommon.Http;

namespace TracksCommon.Gateways
{
    public class RadioParser : IRadioParser
    {
        private readonly IHttpPoster httpPoster;
        private readonly IRadioConfiguration radioConfiguration;
        private readonly ILogGateway logGateway;
        private readonly List<Regex> regexArtist;
        private readonly List<Regex> regexTitle;

        public RadioParser(IHttpPoster httpPoster, IRadioConfiguration radioConfiguration, ILogGateway logGateway)
        {
            this.radioConfiguration = radioConfiguration;
            this.logGateway = logGateway;
            this.httpPoster = httpPoster;
            this.regexArtist = new List<Regex>(radioConfiguration.RegexArtist.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => new Regex(x.Trim())));
            this.regexTitle = new List<Regex>(radioConfiguration.RegexTitle.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => new Regex(x.Trim())));
        }
        public IEnumerable<SongFromRadio> Parse()
        {
            var data = httpPoster.Get(radioConfiguration.RadioUrl);
            if (radioConfiguration.Debug)
                logGateway.AddLog(Log.Info(data));

            data = data.Replace("\\u003c", "<");
            data = data.Replace("\\u003e", ">");
            data = data.Replace(@"<\/", "</");
            data = data.Replace(@"\\x22", "\x22");

            var artist = "";
            foreach (var regexA in regexArtist)
            {
                artist = regexA.Match(data).Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(artist))
                    break;
            }

            var title = "";
            foreach (var regexT in regexTitle)
            {
                title = regexT.Match(data).Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(title))
                    break;
            }

            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title) || title.Contains("La musique revient vite") || title.Contains("Titre non disponible"))
                return null;

            return new[] { new SongFromRadio(artist, title) };
        }
    }
}
