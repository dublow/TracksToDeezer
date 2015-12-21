using System;
using System.Data;
using System.Linq;
using Dapper;
using TracksCommon.Entities;
using TracksCommon.Providers;

namespace TracksCommon.Gateways
{
    public class LogGateway : ILogGateway
    {
        private readonly ISqlConnectionProvider sqlConnectionProvider;

        public LogGateway(ISqlConnectionProvider sqlConnectionProvider)
        {
            this.sqlConnectionProvider = sqlConnectionProvider;
        }


        public void AddLog(Log log)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@Type", log.type);
                dynParameters.Add("@Message", log.message);

                co.Execute("log.Add", dynParameters, commandType: CommandType.StoredProcedure);

                Console.WriteLine("logs Added: {0}", log.message);
            }
        }
    }
}
