namespace TracksCommon.Configurations.Radio
{
    public interface IRadioConfiguration
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
