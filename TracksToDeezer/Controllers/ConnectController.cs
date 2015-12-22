using System;
using System.IO;
using System.Net;
using Siriona.Library.HttpServices;
using TracksCommon.Configurations;
using TracksCommon.Gateways;
using TracksToDeezer.Results;

namespace TracksToDeezer.Controllers
{
    public class ConnectController : Controller
    {
        private readonly IApiGateway apiGateway;
        private readonly IDeezerGateway deezerGateway;
        private readonly IDeezerServerConfiguration configuration;

        public ConnectController(IDeezerServerConfiguration configuration, IApiGateway apiGateway, IDeezerGateway deezerGateway)
        {
            this.apiGateway = apiGateway;
            this.deezerGateway = deezerGateway;
            this.configuration = configuration;
        }

        public ActionResult Auth()
        {
            return new AuthContent(configuration.AppId, configuration.Callback, new[] { "basic_access", "email", "manage_library", "offline_access" });
        }

        public ActionResult Callback()
        {
            var code = this.HttpContext.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                var token = deezerGateway.GetToken(configuration.AppId, configuration.SecretId, code);
                var me = deezerGateway.Me(token);

                apiGateway.RemoveApi("Deezer");
                apiGateway.AddApi("Deezer", token, me.Id, me.Firstname, me.Lastname);

            }
            return NoContent();
        }
    }
}
