using System.Data;

namespace TracksCommon.Providers
{
    public interface ISqlConnectionProvider
    {
        IDbConnection Create();
    }
}
