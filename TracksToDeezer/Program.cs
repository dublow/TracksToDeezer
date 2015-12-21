using System;
using System.Collections.Generic;
using System.Linq;
using Siriona.Library.Hosting;
using Siriona.Library.HttpServices;
using Siriona.Library.Injection;
using Siriona.Library.ServiceBus;
using TracksCommon.Business;
using TracksCommon.Entities;
using TracksCommon.Filters;
using TracksCommon.Gateways;
using TracksCommon.Providers;
using TracksToDeezer.Handlers;

namespace TracksToDeezer
{
    class Program
    {
        static void Main(string[] args)
        {
            

            var conf = new ServerConfiguration();
            var sql = new SqlConnectionProvider(conf.ConnectionString);
            var apiGateway = new ApiGateway(sql);
            ILogGateway logGateway = new LogGateway(sql);
            var deezerGateway = new DeezerGateway(TrackManager.LoadDeezerTrackManagers());
            IStatGateway statGateway = new StatGateway(sql);

            try
            {
                var container = new UnityIocContainer();
                container.RegisterInstance(conf);
                container.RegisterInstance(apiGateway);
                container.RegisterInstance(deezerGateway);
                container.RegisterInstance(statGateway);
                container.RegisterInstance(logGateway);

                var server = HttpServer.Named(string.Format("Siriona.Listener.{0}", conf.ServiceName))
                            .At(string.Format("http://+:80/{0}/", conf.ServiceName))
                            .Bind.Path("{controller}/{action}")
                            .CreateHandlersWith(container)
                            .Create();

                var bus = Bus
                    .Named("TracksToDeezer")
                    .At("msmq://localhost/TracksToDeezer")
                    .ControlAt("msmq://localhost/TracksToDeezer.control")
                    .ErrorsAt("msmq://localhost/TracksToDeezer.errors")
                    .Reliable(false)
                    .CreateHandlersWith(container)
                    .CreateBus();

                container.RegisterInstance(bus);
                bus.Subscribe(new SongHandler(conf, apiGateway, deezerGateway, logGateway, LoadRadios(sql, conf.Radios)));

                var services = new List<IHostedService> { server, bus };

                var serviceDescription = new ServiceDescription(conf.ServiceName, conf.ServiceName, conf.ServiceName);
                Host.Run(services.ToArray(), serviceDescription, args);
            }
            catch (Exception ex )
            {
                logGateway.AddLog(Log.Error(ex.Message));
                throw;
            }
        }

        static Dictionary<string, IRadioBusiness> LoadRadios(ISqlConnectionProvider sql, List<string> radios)
        {
            return radios
                    .ToDictionary<string, string, IRadioBusiness>(radio => radio, radio => new RadioBusiness(new RadioGateway(sql, radio)));
        }
    }
}
