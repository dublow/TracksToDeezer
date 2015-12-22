using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Siriona.Library.Hosting;
using Siriona.Library.HttpServices;
using Siriona.Library.Injection;
using Siriona.Library.ServiceBus;
using TracksCommon.Configurations.Radio;
using TracksCommon.Gateways;
using TracksCommon.Providers;
using TracksFromRadio.Controllers;

namespace TracksFromRadio
{
    class Program
    {
        static void Main(string[] args)
        {
            var conf = new RadioConfiguration();
            var sql = new SqlConnectionProvider(conf.ConnectionString);
            var radioGateway = new RadioGateway(sql, conf.ServiceName);
            var logGateway = new LogGateway(sql);
            var clientBus = ClientBus.Named(conf.ServiceName).Reliable(false).CreateBus();

            var container = new UnityIocContainer();
            container.RegisterInstance(conf);
            container.RegisterInstance(radioGateway);
            container.RegisterInstance(logGateway);
            container.RegisterInstance(clientBus);

            var server = HttpServer.Named(string.Format("Siriona.Listener.{0}", conf.ServiceName))
                        .At(string.Format("http://+:80/{0}/", conf.ServiceName))
                        .Bind.Path("{controller}/{action}")
                        .CreateHandlersWith(container)
                        .Create();

            var serviceDescription = new ServiceDescription(string.Format("Tracks from radio {0}", conf.ServiceName), conf.ServiceName, conf.ServiceName);

            var ctrl = new TrackController(radioGateway, logGateway, clientBus, conf);
            var timer = new System.Timers.Timer(int.Parse(conf.Timer));
            timer.Elapsed += (sender, arguments) =>
            {
                ctrl.Current();
            };

            timer.Start();

            Host.Run(server, serviceDescription, args);
        }
    }
}
