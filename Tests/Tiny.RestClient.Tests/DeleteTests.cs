using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using Tiny.RestClient.ForTest.Api.Models;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class DeleteTests : BaseTest
    {
        [TestMethod]
        public async Task DeleteWithoutResponse()
        {
            var clientFluent = GetClient();
            await clientFluent.
                DeleteRequest("DeleteTest/noResponse").
                ExecuteAsync();
            clientFluent = GetClientXML();
            await clientFluent.
                 DeleteRequest("DeleteTest/noResponse").
                ExecuteAsync();
        }

        [TestMethod]
        public async Task DeleteComplexData()
        {
            uint id = 42;
            string data = "DATA=32";

            var client = GetClient();

            var response = await client.
                 DeleteRequest("DeleteTest/complex").
                 AddQueryParameter("id", id).
                 AddQueryParameter("data", data).
                 ExecuteAsync<Response>();

            Assert.AreEqual(id, (uint)response.Id);
            Assert.AreEqual(data, response.ResponseData);
            client = GetClientXML();
            response = await client.
                DeleteRequest("DeleteTest/complex").
                AddQueryParameter("id", id).
                AddQueryParameter("data", data).
                ExecuteAsync<Response>();

            Assert.AreEqual(id, (uint)response.Id);
            Assert.AreEqual(data, response.ResponseData);

            var streamToSend = new MemoryStream(GetByteArray(id));
            var stream = await client.
                 DeleteRequest("DeleteTest/Stream").
                AddStreamContent(streamToSend).
                ExecuteAsStreamAsync();
            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.Length == id);
        }
    }
}