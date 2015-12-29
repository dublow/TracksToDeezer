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
using TracksCommon.Http;
using TracksCommon.Providers;
using TracksToDeezer.Handlers;
using TracksCommon.Configurations.Deezer;
using TracksCommon.Search;

namespace TracksToDeezer
{
    class Program
    {
        static void Main(string[] args)
        {
            var deezerConfiguration = new DeezerConfiguration();
            var endpoints = deezerConfiguration.Endpoints;
            var sql = new SqlConnectionProvider(deezerConfiguration.ConnectionString);
            var apiGateway = new ApiGateway(sql);
            IHttpPoster httpPoster = new HttpPoster();
            ILogGateway logGateway = new LogGateway(sql);

            var filters = new List<IFilter> { new DeezerFullFilter(), new DeezerTitleFilter(), new DeezerArtistFilter() };
            var searchs = new List<ISearch>
            {
                new FullSearch(endpoints[Endpoint.FullSearch], filters),
                new TitleSearch(endpoints[Endpoint.TitleSearch], filters),
                new ArtistSearch(endpoints[Endpoint.ArtistSearch], filters)
            };

            var deezerGateway = new DeezerGateway(searchs, httpPoster, endpoints);
            IStatGateway statGateway = new StatGateway(sql);

            try
            {
                var container = new UnityIocContainer();
                container.RegisterInstance(deezerConfiguration);
                container.RegisterInstance(apiGateway);
                container.RegisterInstance(deezerGateway);
                container.RegisterInstance(statGateway);
                container.RegisterInstance(logGateway);

                var server = HttpServer.Named(string.Format("Siriona.Listener.{0}", deezerConfiguration.ServiceName))
                            .At(string.Format("http://+:80/{0}/", deezerConfiguration.ServiceName))
                            .Bind.Path("{controller}/{action}")
                            .CreateHandlersWith(container)
                            .Create();

                var bus = Bus
                    .Named("TracksToDeezer")
                    .CreateHandlersWith(container)
                    .DisableSubscriptionService()
                    .CreateBus();

                container.RegisterInstance(bus);
                bus.Subscribe(new SongHandler(deezerConfiguration, apiGateway, deezerGateway, logGateway, LoadRadios(sql, deezerConfiguration.Radios)));

                var services = new List<IHostedService> { server, bus };

                var serviceDescription = new ServiceDescription(deezerConfiguration.ServiceName, deezerConfiguration.ServiceName, deezerConfiguration.ServiceName);
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
