using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public static class Program
    {
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

            Client?.Dispose();
        }

        public static HttpClient Client { get; set; }
    }
}
