using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tiny.RestClient.ForTest.Api.Models;

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

            Assert.AreEqual(postRequest.Id, response.Id, "id doesn't match");
            Assert.AreEqual(postRequest.Data, response.ResponseData, "data doesn't match");
        }

        [TestMethod]
        public async Task GzipResponse()
        {
            var client = GetNewClient();
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
