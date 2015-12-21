using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksCommon.Gateways
{
    public interface ILogGateway
    {
        void AddLog(Log log);
    }
}
