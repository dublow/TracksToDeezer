using System.Data;
using System.Data.SqlClient;
using Siriona.Library.Data;

namespace TracksCommon.Providers
{
    public class SqlConnectionProvider : ISqlConnectionProvider
    {
        private readonly string connectionString;

        public SqlConnectionProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            return new SqlConnection(connectionString);
        }

        public static ISqlConnectionProvider CreateConnectionProvider(string connectionName)
        {
            var coString = ConnectionSettings.FromName(connectionName).ConnectionString;
            return new SqlConnectionProvider(coString);
        }
    }
}
