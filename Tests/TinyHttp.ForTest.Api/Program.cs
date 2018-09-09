using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Tiny.RestClient.ForTest.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).
                UseUrls("http://localhost:4242").
                Build().
                Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
