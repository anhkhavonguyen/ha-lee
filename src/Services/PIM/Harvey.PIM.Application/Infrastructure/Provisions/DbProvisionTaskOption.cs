using System;

namespace Harvey.PIM.Application.Infrastructure.Provisions
{
    public class DbProvisionTaskOption : ITaskOption
    {
        public string ConnectionString { get; }
        public DbProvisionTaskOption(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
