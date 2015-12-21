using System;
using Siriona.Library.EventModel;

namespace TracksCommon.Events
{
    [Serializable]
    public class SongAdded : Event
    {
        public readonly int id;
        public readonly string media;
        public readonly string artist;
        public readonly string title;

        public SongAdded(int id, string media, string artist, string title)
        {
            this.id = id;
            this.media = media;
            this.artist = artist;
            this.title = title;
        }
    }
}
