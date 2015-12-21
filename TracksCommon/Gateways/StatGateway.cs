using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TracksCommon.Providers;

namespace TracksCommon.Gateways
{
    public class StatGateway : IStatGateway
    {
        private readonly ISqlConnectionProvider sqlConnectionProvider;

        public StatGateway(ISqlConnectionProvider sqlConnectionProvider)
        {
            this.sqlConnectionProvider = sqlConnectionProvider;
        }

        public object StatRadioByDays()
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var result = co.Query<object>("stat.RadioByDay", null, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public object StatRadioCurrentDay()
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var result = co.Query<object>("stat.RadioCurrentDay", null, commandType: CommandType.StoredProcedure).SingleOrDefault();
                return result;
            }
        }


        public object StatTrackByRadio()
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var result = co.Query<object>("stat.TracksByRadio", null, commandType: CommandType.StoredProcedure).SingleOrDefault();
                return result;
            }
        }
    }
}
