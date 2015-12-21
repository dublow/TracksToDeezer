using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Siriona.Library.HttpServices;
using Siriona.Library.ServiceBus;
using TracksCommon.Configurations;
using TracksCommon.Entities;
using TracksCommon.Events;
using TracksCommon.Gateways;

namespace TracksFromRadio.Controllers
{
    public class TrackController : Controller
    {
        private SongFromRadio LastTrack { get; set; }
        private readonly IRadioGateway radioGateway;
        private readonly ILogGateway logGateway;
        private readonly IClientBus bus;
        private readonly IServerConfiguration serverConfiguration;
        private readonly string radioUrl;
        private readonly List<Regex> regexArtist;
        private readonly List<Regex> regexTitle;

        public TrackController(IRadioGateway radioGateway, ILogGateway logGateway, IClientBus bus, IServerConfiguration serverConfiguration)
        {
            this.radioGateway = radioGateway;
            this.logGateway = logGateway;
            this.bus = bus;
            this.serverConfiguration = serverConfiguration;
            this.radioUrl = serverConfiguration.RadioUrl;
            this.regexArtist = new List<Regex>(serverConfiguration.RegexArtist.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => new Regex(x.Trim())));
            this.regexTitle = new List<Regex>(serverConfiguration.RegexTitle.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => new Regex(x.Trim())));
        }
        public ActionResult Current()
        {
            try
            {
                var songFromRdo = GetSong();
                if (songFromRdo != null)
                {
                    if (LastTrack != null && LastTrack.artist == songFromRdo.artist &&
                        LastTrack.title == songFromRdo.title)
                    {
                        Console.WriteLine("[Memory] Same Tracks: {0}", songFromRdo.title);
                        return NoContent();
                    }

                    LastTrack = songFromRdo;
                    var songFromDb = radioGateway.GetSong(LastTrack.artist, LastTrack.title);

                    if (songFromDb != null)
                    {
                        Console.WriteLine("[Db] Same Tracks: {0}", songFromDb.title);
                        radioGateway.AddAlreadyPlayed(songFromDb.id);
                        return NoContent();
                    }

                    var songId = radioGateway.Add(songFromRdo);

                    bus.Publish(new SongAdded(songId, radioGateway.Name, LastTrack.artist, LastTrack.title));
                }
                else
                {
                    Console.WriteLine("Not found");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("TracksFromRadio.Controllers {0}", ex.Message);
                logGateway.AddLog(Log.Error(errorMessage));
                Console.WriteLine(errorMessage);
            }

            return NoContent();
        }

        private SongFromRadio GetSong()
        {
            var response = GetResponse();
            if(serverConfiguration.Debug)
                logGateway.AddLog(Log.Info(response));

            response = response.Replace("\\u003c", "<");
            response = response.Replace("\\u003e", ">");
            response = response.Replace(@"<\/", "</");
            response = response.Replace(@"\\x22", "\x22");

            var artist = "";
            foreach (var regexA in regexArtist)
            {
                artist = regexA.Match(response).Groups[1].Value.Trim();
                if(!string.IsNullOrEmpty(artist))
                    break;
            }

            var title = "";
            foreach (var regexT in regexTitle)
            {
                title = regexT.Match(response).Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(title))
                    break;
            }

            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title) || title.Contains("La musique revient vite") || title.Contains("Titre non disponible"))
                return null;

            return new SongFromRadio(artist, title);
        }

        private string GetResponse()
        {
            var webRequest =
                WebRequest.Create(radioUrl);

            using (var webresponse = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(webresponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
