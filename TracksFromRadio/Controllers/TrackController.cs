using System;
using Siriona.Library.HttpServices;
using Siriona.Library.ServiceBus;
using TracksCommon.Configurations.Radio;
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
        private readonly IRadioConfiguration radioConfiguration;
        private readonly IRadioParser radioParser;

        public TrackController(IRadioGateway radioGateway, ILogGateway logGateway, IClientBus bus, 
            IRadioConfiguration radioConfiguration, IRadioParser radioParser)
        {
            this.radioGateway = radioGateway;
            this.logGateway = logGateway;
            this.bus = bus;
            this.radioConfiguration = radioConfiguration;
            this.radioParser = radioParser;
        }
        public ActionResult Current()
        {
            try
            {
                var songFromRdos = radioParser.Parse();

                if (songFromRdos != null)
                {
                    foreach (var songFromRdo in songFromRdos)
                    {
                        if (LastTrack != null && LastTrack.artist == songFromRdo.artist &&
                        LastTrack.title == songFromRdo.title)
                        {
                            Console.WriteLine("[Memory] Same Tracks: {0}", songFromRdo.title);
                            continue;
                        }

                        LastTrack = songFromRdo;
                        var songFromDb = radioGateway.GetSong(LastTrack.artist, LastTrack.title);

                        if (songFromDb != null)
                        {
                            Console.WriteLine("[Db] Same Tracks: {0}", songFromDb.title);
                            radioGateway.AddAlreadyPlayed(songFromDb.id);
                            continue;
                        }

                        var songId = radioGateway.Add(songFromRdo);

                        bus.Publish(new SongAdded(songId, radioGateway.Name, LastTrack.artist, LastTrack.title));
                    }
                    
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
    }
}
