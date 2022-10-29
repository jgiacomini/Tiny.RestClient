using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tiny.RestClient.ForTest.Api.Models;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class DeflateTests : BaseTest
    {
        [TestMethod]
        public async Task DelfateContent()
        {
            var postRequest = new Request
            {
                Id = 42,
                Data = "DATA"
            };

            var client = GetClient();
            var response = await client.
                PostRequest("PostTest/complex", postRequest, compression: client.Settings.Compressions["deflate"]).
                ExecuteAsync<Response>();

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
        }

        [TestMethod]
        public async Task DeflateNoResponse()
        {
            var client = GetClient();
            var data = await client.
                GetRequest("GetTest/noResponse").
                AddHeader("Accept-Encoding", "deflate").
                ExecuteAsStringAsync();
        }

        [TestMethod]
        public async Task DeflateResponse()
        {
            var client = GetNewClient();
            var compression = client.Settings.Compressions["deflate"];
            compression.AddAcceptEncodingHeader = true;
            var data = await client.
                GetRequest("GetTest/Complex").
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }
    }
}
