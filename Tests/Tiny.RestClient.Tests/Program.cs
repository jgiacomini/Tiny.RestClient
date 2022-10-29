using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tiny.RestClient.ForTest.Api;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public static class Program
    {
        private static TestServer _server;

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            var application = new WebApplicationFactory<TestProgram>()
               .WithWebHostBuilder(builder =>
               {
                   builder.ConfigureServices(services =>
                   {
                   });
               });

            Client = application.CreateClient();
        }

        [AssemblyCleanup]
        public static async Task Cleanup()
        {
            var client = BaseTest.GetClient();
            var postManListener = client.Settings.Listeners.OfType<PostmanListener>().First();
            await postManListener.SaveAsync(new System.IO.FileInfo("test_postMan.json"));

            _server?.Dispose();
            Client?.Dispose();
        }

        public static HttpClient Client { get; set; }
    }
}
