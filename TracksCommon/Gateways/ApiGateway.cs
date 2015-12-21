using System.Data;
using System.Linq;
using Dapper;
using TracksCommon.Entities;
using TracksCommon.Providers;

namespace TracksCommon.Gateways
{
    public class ApiGateway : IApiGateway
    {
        private readonly ISqlConnectionProvider sqlConnectionProvider;

        public ApiGateway(ISqlConnectionProvider sqlConnectionProvider)
        {
            this.sqlConnectionProvider = sqlConnectionProvider;
        }

        public void AddApi(string apiName, string token, string userId, string firstName, string lastname)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@ApiName", apiName);
                dynParameters.Add("@Token", token);
                dynParameters.Add("@UserId", userId);
                dynParameters.Add("@FirstName", firstName);
                dynParameters.Add("@Lastname", lastname);

                co.Execute("api.Add", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void RemoveApi(string apiName)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@ApiName", apiName);

                co.Execute("api.Remove", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Api GetApi(string apiName)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@ApiName", apiName);

                return co.Query<Api>("api.Get", dynParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public void AddPlaylist(string title, string id)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@Title", title);
                dynParameters.Add("@Id", id);

                co.Execute("api.AddPlaylist", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Playlist GetPlaylist(string title)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@Title", title);

                return co.Query<Playlist>("api.GetPlaylist", dynParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
    }
}
