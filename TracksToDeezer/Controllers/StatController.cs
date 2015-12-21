using System;
using System.IO;
using Siriona.Library.HttpServices;
using TracksCommon.Entities;
using TracksCommon.Extensions;
using TracksCommon.Gateways;

namespace TracksToDeezer.Controllers
{
    public class StatController : Controller
    {
        private readonly IStatGateway statGateway;
        private readonly ILogGateway logGateway;

        public StatController(IStatGateway statGateway, ILogGateway logGateway)
        {
            this.statGateway = statGateway;
            this.logGateway = logGateway;
        }

        public ActionResult Get()
        {
            try
            {
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                var htmlPath = AppDomain.CurrentDomain.BaseDirectory + @"Views\Stat.html";

                var result = File.ReadAllText(htmlPath);
                result = result.Replace("{statday}", statGateway.StatRadioByDays().ToJson());
                result = result.Replace("{statrdo}", statGateway.StatTrackByRadio().ToJson());
                return Content(result, "text/html; charset=utf-8");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("TracksToDeezer.Controllers.Get {0}", ex.Message);
                logGateway.AddLog(Log.Error(errorMessage));
                Console.WriteLine(errorMessage);
            }

            return NoContent();
        }

        public ActionResult Async()
        {
            try
            {
                var stat = new
                {
                    day = statGateway.StatRadioCurrentDay(),
                    rdo = statGateway.StatTrackByRadio()
                }.ToJson();
                return Content(stat, "text/html; charset=utf-8");
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("TracksToDeezer.Controllers.Async {0}", ex.Message);
                logGateway.AddLog(Log.Error(errorMessage));
                Console.WriteLine(errorMessage);
            }

            return NoContent();
        }

        public ActionResult Chart()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var jsPath = AppDomain.CurrentDomain.BaseDirectory + @"Views\chart.js";

            return Content(File.ReadAllText(jsPath));
        }
    }
}
