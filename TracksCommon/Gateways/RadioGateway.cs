using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using TracksCommon.Entities;
using TracksCommon.Providers;

namespace TracksCommon.Gateways
{
    public class RadioGateway : IRadioGateway
    {
        public string Name { get; private set; }

        private readonly ISqlConnectionProvider sqlConnectionProvider;

        public RadioGateway(ISqlConnectionProvider sqlConnectionProvider, string name)
        {
            this.sqlConnectionProvider = sqlConnectionProvider;
            this.Name = name;
        }

        public int Add(SongFromRadio song)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioName", Name);
                dynParameters.Add("@Artist", song.artist);
                dynParameters.Add("@Title", song.title);
                dynParameters.Add("@new_identity", dbType: DbType.Int32, direction: ParameterDirection.Output);

                co.Execute("track.Add", dynParameters, commandType: CommandType.StoredProcedure);

                Console.WriteLine("Tracks Added: {0}", song.title);

                return dynParameters.Get<int>("@new_identity");
            }
        }

        public void AddGenre(Genre genre)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@Id", genre.Id);
                dynParameters.Add("@Name", genre.Name);

                co.Execute("track.AddGenre", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public SongFromDb GetSong(string artist, string title)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioName", Name);
                dynParameters.Add("@Artist", artist);
                dynParameters.Add("@Title", title);

                return
                    co.Query<SongFromDb>("track.Get", dynParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<SongFromDb> GetAllSongIsNull()
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioName", Name);

                return co.Query<SongFromDb>("track.GetAllIsNull", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<SongFromDb> GetAllSongIsUnpublished()
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioName", Name);

                return co.Query<SongFromDb>("track.GetAllIsUnpublished", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<SongFromDb> GetAllHasNoGenre()
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioName", Name);

                return co.Query<SongFromDb>("track.GetAllHasNoGenre", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(int id, string trackId, string message)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioName", Name);
                dynParameters.Add("@Id", id);
                dynParameters.Add("@TrackId", trackId);
                dynParameters.Add("@Message", message);

                co.Execute("track.Update", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateTrackId(int id, string trackId)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioName", Name);
                dynParameters.Add("@Id", id);
                dynParameters.Add("@TrackId", trackId);

                co.Execute("track.UpdateTrackId", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddAlreadyPlayed(int radioId)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioId", radioId);
                dynParameters.Add("@RadioName", Name);

                co.Execute("track.AddAlreadyPlayed", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddGenreToTrack(int trackId, int genreId)
        {
            using (var co = sqlConnectionProvider.Create())
            {
                var dynParameters = new DynamicParameters();
                dynParameters.Add("@RadioName", Name);
                dynParameters.Add("@GenreId", genreId);
                dynParameters.Add("@TrackId", trackId);

                co.Execute("track.AddGenreToTrack", dynParameters, commandType: CommandType.StoredProcedure);
            }
        }


        
    }
}
