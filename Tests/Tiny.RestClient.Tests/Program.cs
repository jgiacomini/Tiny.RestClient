using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            _server = new TestServer(new WebHostBuilder().
                UseUrls("http://localhost:4242").
                UseStartup<Startup>());

            Client = _server.CreateClient();
        }

        [AssemblyCleanup]
        public static async Task Cleanup()
        {
            var client = BaseTest.GetClient();
            var postManListener = client.Settings.Listeners.OfType<PostManListener>().First();
            await postManListener.SaveAsync(new System.IO.FileInfo("test_postMan.json"));

            _server?.Dispose();
            Client?.Dispose();
        }

        public static HttpClient Client { get; set; }
    }
}
