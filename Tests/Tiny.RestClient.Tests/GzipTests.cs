using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class GzipTests : BaseTest
    {
        [TestMethod]
        public async Task Gzip()
        {
            var client = GetClient();
            client.Settings.Listeners.AddDebug();

            var compression = client.Settings.Compressions["gzip"];
            compression.AddAcceptEncodingHeader = true;

            var postman = client.Settings.Listeners.AddPostman("gzip");
            var data = await client.
                GetRequest("GetTest/Complex").
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            await postman.SaveAsync(new FileInfo("gzip.json"));
        }

        [TestMethod]
        public async Task NoResponseGzip()
        {
            var client = GetClient();
            var postman = client.Settings.Listeners.AddPostman("gzip");
            var data = await client.
                GetRequest("GetTest/noResponse").
                AddHeader("Accept-Encoding", "gzip").
                ExecuteAsStringAsync();

            await postman.SaveAsync(new FileInfo("gzip.json"));
        }
    }
}
