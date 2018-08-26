using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using Tiny.Http.Tests.Models;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class DeleteTests : BaseTest
    {
        [TestMethod]
        public async Task DeleteWithoutResponse()
        {
            var clientFluent = GetClient();
            await clientFluent.
                NewRequest(HttpVerb.Delete, "DeleteTest/noResponse").
                ExecuteAsync();
            clientFluent = GetClientXML();
            await clientFluent.
                NewRequest(HttpVerb.Delete, "DeleteTest/noResponse").
                ExecuteAsync();
        }

        [TestMethod]
        public async Task DeleteComplexData()
        {
            int id = 42;
            string data = "DATA=32";

            var client = GetClient();

            var response = await client.
                NewRequest(HttpVerb.Delete, "DeleteTest/complex").
                AddQueryParameter("id", id).
                AddQueryParameter("data", data).
                ExecuteAsync<Response>();

            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
            client = GetClientXML();
            response = await client.
                NewRequest(HttpVerb.Delete, "DeleteTest/complex").
                AddQueryParameter("id", id).
                AddQueryParameter("data", data).
                ExecuteAsync<Response>();

            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);

            var byteArray = new byte[id];
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = i % 2 == 0 ? (byte)0 : (byte)1;
            }

            var streamToSend = new MemoryStream(byteArray);
            var stream = await client.
                NewRequest(HttpVerb.Delete, "DeleteTest/Stream").
                AddStreamContent(streamToSend).
                WithStreamResponse().
                ExecuteAsync();
            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.Length == id);
        }
    }
}