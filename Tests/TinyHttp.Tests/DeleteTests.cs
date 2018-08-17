using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Tiny.Http.Models;

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
            string data = "DATA";

            var clientFluent = GetClient();
            var response = await clientFluent.
                NewRequest(HttpVerb.Delete, "DeleteTest/complex").
                AddQueryParameter("id", id).
                AddQueryParameter("data", data).
                ExecuteAsync<PostResponse>();

            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
            clientFluent = GetClientXML();
            response = await clientFluent.
                NewRequest(HttpVerb.Delete, "DeleteTest/complex").
                AddQueryParameter("id", id).
                AddQueryParameter("data", data).
                ExecuteAsync<PostResponse>();

            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
        }
    }
}