using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args) => 
            CreateWebHostBuilder(args)
               .Build()
               .Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.ConfigureLogging(
                //    (host, log) =>
                //    {
                //        //log.ClearProviders();
                //        log.AddConsole().AddFilter("Microsoft", LogLevel.Error);
                //        log.AddDebug().AddFilter((category, level) => category == "Default" && level == LogLevel.Error);

                //    })
                //.UseUrls("http://0.0.0.0:8080")
                .UseStartup<Startup>();
    }
}
