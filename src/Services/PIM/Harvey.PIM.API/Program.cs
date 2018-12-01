using Microsoft.Extensions.DependencyInjection;
using Harvey.PIM.Application.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Harvey.PIM.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .MigrateDbContext<PimDbContext>((context, services) =>
                {
                    var logger = services.GetService<ILogger<PimDbContextDataSeed>>();
                    new PimDbContextDataSeed().SeedAsync(context, logger).Wait();
                })
                .MigrateDbContext<ActivityLogDbContext>((context, services) =>
                {
                    var logger = services.GetService<ILogger<ActivityLogDataSeed>>();
                    new ActivityLogDataSeed().SeedAsync(context, logger).Wait();
                })
                .MigrateDbContext<TransactionDbContext>((context, services) =>
                {
                    var logger = services.GetService<ILogger<TransactionDbContextDataSeed>>();
                    new TransactionDbContextDataSeed().SeedAsync(context, logger).Wait();
                })
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false);
    }
}
