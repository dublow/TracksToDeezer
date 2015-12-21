using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace TracksCommon.Gateways
{
    public interface IStatGateway
    {
        object StatRadioByDays();
        object StatRadioCurrentDay();
        object StatTrackByRadio();
    }
}
