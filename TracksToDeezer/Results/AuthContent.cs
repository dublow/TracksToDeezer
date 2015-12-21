using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Siriona.Library.HttpServices;

namespace TracksToDeezer.Results
{
    public class AuthContent : ActionResult
    {
        private readonly string appId;
        private readonly string redirectUri;
        private readonly string perms;

        public AuthContent(string appId, string redirectUri, params string[] perms)
        {
            this.appId = appId;
            this.redirectUri = redirectUri;
            this.perms = perms.Aggregate((c, n) => c + ", " + n);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Redirect(string.Format("https://connect.deezer.com/oauth/auth.php?app_id={0}&redirect_uri={1}&perms={2}", appId,
                    redirectUri, perms));
        }
    }
}
