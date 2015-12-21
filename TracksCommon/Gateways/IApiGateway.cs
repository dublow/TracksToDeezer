using System;
using System.Reflection.Emit;
using TracksCommon.Entities;

namespace TracksCommon.Gateways
{
    public interface IApiGateway
    {
        void AddApi(string apiName, string token, string userId, string firstName, string lastname);
        void RemoveApi(string apiName);
        Api GetApi(string apiName);
        void AddPlaylist(string title, string id);
        Playlist GetPlaylist(string title);
    }
}
