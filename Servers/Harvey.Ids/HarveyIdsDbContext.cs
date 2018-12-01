using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Harvey.Ids.Domains;

namespace Harvey.Ids
{
    public class HarveyIdsDbContext: IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public DbSet<ShortLink> ShortLinks { get; set; }

        public HarveyIdsDbContext(DbContextOptions<HarveyIdsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
