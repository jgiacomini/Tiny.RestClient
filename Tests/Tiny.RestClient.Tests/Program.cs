using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
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
        public static void Cleanup()
        {
            _server?.Dispose();
            Client?.Dispose();
        }

        public static HttpClient Client { get; set; }
    }
}
