using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
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
    public class HistoricRadioParser : IRadioParser
    {
        private readonly IHttpPoster httpPoster;
        private readonly IRadioConfiguration radioConfiguration;
        private readonly ILogGateway logGateway;
        private readonly List<Regex> regexArtist;
        private readonly List<Regex> regexTitle;

        public HistoricRadioParser(IHttpPoster httpPoster, IRadioConfiguration radioConfiguration, ILogGateway logGateway)
        {
            this.radioConfiguration = radioConfiguration;
            this.logGateway = logGateway;
            this.httpPoster = httpPoster;
            this.regexArtist = new List<Regex>(radioConfiguration.RegexArtist.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => new Regex(x.Trim())));
            this.regexTitle = new List<Regex>(radioConfiguration.RegexTitle.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => new Regex(x.Trim())));
        }
        public IEnumerable<SongFromRadio> Parse()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("display_search", "1");
            dic.Add("select_radio", "7");
            dic.Add("select_jour", "1452034800");
            dic.Add("select_heure", "00");
            dic.Add("select_minute", "00");
            dic.Add("form_build_id", "form-VKNiOPfykzJa9k7_JdRAF3YgvI1bxqf7KubHisP8tEU");
            dic.Add("form_id", "fip_titres_diffuses_cruiser_form_search_date");

            var listDate = new Dictionary<double, List<int>>();
            var listSong = new List<SongFromRadio>();
            for (int i = -7; i <= 0 ; i++)
            {
                var itemDate = DateTime.UtcNow.Date.AddDays(i);
                var dateKey = (itemDate.Subtract(new DateTime(1970, 1, 1).AddHours(1))).TotalSeconds;

                dic["select_jour"] = dateKey.ToString();

                foreach (var itemRange in Enumerable.Range(0, 24).ToList())
                {
                    dic["select_heure"] = itemRange.ToString();

                    foreach (var itemMinRange in Enumerable.Range(0, 2).ToList())
                    {
                        dic["select_minute"] = (itemMinRange % 2) == 0 ? "00" : "30";

                        var data = httpPoster.Post(radioConfiguration.RadioUrl, dic);

                        var jObj = JArray.Parse(data)
                            .Where(x => x["data"] != null && !string.IsNullOrEmpty(x["data"].Value<string>()))
                            .Select(x => x["data"]).SingleOrDefault();

                        if (jObj != null)
                        {
                            var s = jObj.Value<string>();
                            var htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(s);

                            var nodes = htmlDocument.DocumentNode.SelectNodes("//div[@class=\"texte\"]");

                            if (nodes == null)
                            {
                                Console.WriteLine("[{0}][{1}:{2}] not found", itemDate.Date, itemRange, dic["select_minute"]);
                                continue;
                            }

                            Console.WriteLine("[{0}][{1}:{2}] found", itemDate.Date, itemRange, dic["select_minute"]);
                            var g = nodes.ToLookup(
                                    k => k.ChildNodes.Where(x => x.GetAttributeValue("class", "null") == "titre_title").Select(y => y.InnerText).SingleOrDefault(),
                                    e => e.ChildNodes.Where(x => x.GetAttributeValue("class", "null") == "titre_artiste").Select(y => y.InnerText).SingleOrDefault());

                            foreach (var item in g)
                            {
                                var artist = item.FirstOrDefault() == null ? "NoArtist" : item.FirstOrDefault().Replace("Par", "").Replace(":", "").Replace("\n", "").Trim();
                                var title = item.Key == null ? "NoTitle" : item.Key.Trim();
                                listSong.Add(new SongFromRadio(artist, title));
                            }

                        }


                    }
                }
            }

            return listSong;
        }
    }
}
