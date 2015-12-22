namespace TracksCommon.Entities
{
    public class DeezerSearchItem : IDeserializer
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
        public bool IsEmpty { get { return Id == "-1"; } }

        public static DeezerSearchItem Empty
        {
            get { return new DeezerSearchItem {Id = "-1"}; }
        }
    }
}
