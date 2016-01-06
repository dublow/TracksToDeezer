namespace TracksCommon.Entities
{
    public class SongFromDb
    {
        public readonly int id;
        public readonly string trackId;
        public readonly string artiste;
        public readonly string title;

        
        public SongFromDb(int id, string trackId, string artiste, string title)
        {
            this.id = id;
            this.trackId = trackId;
            this.artiste = artiste;
            this.title = title;
        }
    }
}
