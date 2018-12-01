using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Harvey.PurchaseControl.Application.Infrastructure;

namespace Harvey.PurchaseControl.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
               .MigrateDbContext<PurchaseControlDbContext>((context, services) =>
               {
                   var logger = services.GetService<ILogger<PurchaseControlDbContext>>();
                   new PurchaseControlDbContextDataSeed().SeedAsync(context, logger).Wait();
               })
               .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
