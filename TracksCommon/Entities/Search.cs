namespace TracksCommon.Entities
{
    public class Search
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
    }
}
