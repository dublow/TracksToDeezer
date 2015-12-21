namespace TracksCommon.Configurations
{
    public interface IServerConfiguration
    {
        string ServiceName { get; }
        string ConnectionString { get; }
        string Timer { get; }
        string RadioUrl { get; }
        string RegexArtist { get; }
        string RegexTitle { get; }
        bool Debug { get; }
    }
}
