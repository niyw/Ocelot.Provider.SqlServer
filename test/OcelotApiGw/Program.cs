using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;

namespace OcelotApiGw {
    public class Program {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config
                                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                                .AddJsonFile("appsettings.json", true, true)
                                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                                //.AddJsonFile("ocelot.json", optional:false, reloadOnChange:true)
                                .AddOcelot(hostingContext.HostingEnvironment)
                                .AddEnvironmentVariables();
                        })
                .UseStartup<Startup>();
    }
}
