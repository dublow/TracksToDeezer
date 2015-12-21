using System.Collections.Generic;
using TracksCommon.Entities;

namespace TracksCommon.Gateways
{
    public interface IRadioGateway
    {
        string Name { get; }
        int Add(SongFromRadio song);
        void AddGenre(Genre genre);
        SongFromDb GetSong(string artist, string title);
        IEnumerable<SongFromDb> GetAllSongIsNull();
        IEnumerable<SongFromDb> GetAllSongIsUnpublished();
        IEnumerable<SongFromDb> GetAllHasNoGenre(); 
        void Update(int id, string trackId, string message);
        void UpdateTrackId(int id, string trackId);
        void AddAlreadyPlayed(int radioId);
        void AddGenreToTrack(int trackId, int genreId);
    }
}
