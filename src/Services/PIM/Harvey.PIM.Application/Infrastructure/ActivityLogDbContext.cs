using Harvey.Logging;
using Microsoft.EntityFrameworkCore;

namespace Harvey.PIM.Application.Infrastructure
{
    public class ActivityLogDbContext : DbContext
    {
        public ActivityLogDbContext(DbContextOptions<ActivityLogDbContext> options) : base(options)
        {

        }

        public DbSet<ActivityLog> ActivityLogs { get; set; }
    }
}
