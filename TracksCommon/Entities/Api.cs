using System;

namespace TracksCommon.Entities
{
    public class Api
    {
        public readonly string apiName;
        public readonly string token;
        public readonly string userId;
        public readonly string firstname;
        public readonly string lastname;

        public Api(string apiName, string token, string userId, string firstname, string lastname)
        {
            this.apiName = apiName;
            this.token = token;
            this.userId = userId;
            this.firstname = firstname;
            this.lastname = lastname;
        }
    }
}
