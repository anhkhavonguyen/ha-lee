namespace Harvey.Logging
{
    public interface ICentralizeLoggingConfiguration : ILoggingConfiguration
    {
        string Url { get; set; }
    }
}
