using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            await client.NewRequest(HttpVerb.Delete, "toto").AddHeader("toto", "tutu").WithByteArrayResponse().ExecuteAsync();

            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
        }
    }
}