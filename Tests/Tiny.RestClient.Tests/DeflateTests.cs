using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class DeflateTests : BaseTest
    {
        // [TestMethod]
        public async Task DeflateNoResponse()
        {
            var client = GetClient();
            var postman = client.Settings.Listeners.AddPostman("deflate");
            var data = await client.
                GetRequest("GetTest/noResponse").
                AddHeader("Accept-Encoding", "deflate").
                ExecuteAsStringAsync();

            await postman.SaveAsync(new FileInfo("deflate.json"));
        }

        // [TestMethod]
        public async Task Deflate()
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var httpClient = new System.Net.Http.HttpClient(handler);
            var str = await httpClient.GetStringAsync("http://httpbin.org/deflate");

            var client = new TinyRestClient(httpClient, "http://httpbin.org/");
            var postman = client.Settings.Listeners.AddPostman("deflate");
            var data = await client.
                GetRequest("deflate").
                AddHeader("Accept-Encoding", "deflate").
                FillResponseHeaders(out Headers headers).
                ExecuteAsync<string[]>();

            await postman.SaveAsync(new FileInfo("deflate.json"));
        }
    }
}
