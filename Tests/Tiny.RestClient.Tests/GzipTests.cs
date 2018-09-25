using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using Tiny.RestClient.Tests.Models;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class GzipTests : BaseTest
    {
        [TestMethod]
        public async Task GzipContent()
        {
            var postRequest = new Request
            {
                Id = 42,
                Data = "DATA"
            };

            var client = GetClient();
            var response = await client.
                PostRequest("PostTest/complex", postRequest, compression: client.Settings.Compressions["gzip"]).
                ExecuteAsync<Response>();

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
        }

        [TestMethod]
        public async Task GzipResponse()
        {
            var client = GetClient();
            var compression = client.Settings.Compressions["gzip"];
            compression.AddAcceptEncodingHeader = true;
            var data = await client.
                GetRequest("GetTest/Complex").
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }

        [TestMethod]
        public async Task NoResponseGzip()
        {
            var client = GetClient();
            var data = await client.
                GetRequest("GetTest/noResponse").
                AddHeader("Accept-Encoding", "gzip").
                ExecuteAsStringAsync();
        }
    }
}
