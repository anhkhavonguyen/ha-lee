using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Ocelot.DependencyInjection;
using System.Threading.Tasks;

namespace Harvey.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Delay(20000).Wait();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext,config)=> {
                    config.AddOcelot();
                })
                .UseStartup<Startup>();
    }
}
