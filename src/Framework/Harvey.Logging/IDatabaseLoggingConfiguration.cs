namespace Harvey.Logging
{
    public interface IDatabaseLoggingConfiguration : ILoggingConfiguration
    {
        string ConnectionString { get; set; }
        string TableName { get; set; }
    }
}
