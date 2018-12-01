using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.Polly;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure
{
    public class PimDbContextDataSeed
    {
        public async Task SeedAsync(PimDbContext context, ILogger<PimDbContextDataSeed> logger)
        {
            var policy = new DataSeedRetrivalPolicy();
            await policy.ExecuteStrategyAsync(logger, () =>
            {
                using (context)
                {
                    var brand = GetPreconfiguredEntityRef().Single(x => x.Name == "Brand");
                    if (context.EntityRefs.FirstOrDefault(x => brand.Name == x.Name) == null)
                    {
                        context.EntityRefs.Add(brand);
                        context.SaveChanges();
                    }
                    if(!context.Locations.Any())
                    {
                        context.Locations.AddRange(GetPreconfiguredLocation());
                    }

                    context.SaveChanges();
                }
            });
        }

        private List<EntityRef> GetPreconfiguredEntityRef()
        {
            return new List<EntityRef>()
            {
                new EntityRef()
                {
                    Name =  "Brand",
                    Namespace = typeof(Brand).AssemblyQualifiedName
                }
            };
        }

        private List<Location> GetPreconfiguredLocation()
        {
            return new List<Location>()
            {
                new Location()
                {
                    Id = Guid.Parse("508c1b19-5e5d-440b-9414-42f7de8879d4"),
                    Name = "Outside",
                    Address = "Outside",
                    CreatedDate = DateTime.UtcNow
                }
            };
        }
    }
}
