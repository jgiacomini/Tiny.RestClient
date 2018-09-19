using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class CompressionTests : BaseTest
    {
        [TestMethod]
        public async Task Gzip()
        {
            var client = GetClient();
            var postman = client.Settings.Listeners.AddPostman("gzip");
            var data = await client.
                GetRequest("GetTest/Complex").
                AddHeader("Accept-Encoding", "gzip").
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

        ////[TestMethod]
        ////public async Task Deflate()
        ////{
        ////    var client = new TinyRestClient(new System.Net.Http.HttpClient(), "http://httpbin.org/");
        ////    var postman = client.Settings.Listeners.AddPostman("deflate");
        ////    var data = await client.
        ////        GetRequest("deflate").
        ////        AddHeader("Accept-Encoding", "deflate").
        ////        FillResponseHeaders(out Headers headers).
        ////        ExecuteAsStringAsync();

        ////    await postman.SaveAsync(new FileInfo("deflate.json"));
        ////}
    }
}
