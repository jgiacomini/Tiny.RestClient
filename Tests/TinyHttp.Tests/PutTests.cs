using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Tiny.Http.Tests.Models;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class PutTests : BaseTest
    {
        [TestMethod]
        public async Task PutWithoutResponse()
        {
            var postRequest = new Request
            {
                Id = 42,
                Data = "DATA"
            };

            var client = GetClient();
            await client.
                NewRequest(HttpVerb.Put, "PutTest/noResponse").
                AddContent(postRequest).
                ExecuteAsync();

            client = GetClientXML();
            await client.
                NewRequest(HttpVerb.Put, "PutTest/noResponse").
                AddContent(postRequest).
                SerializeWith(new TinyXmlSerializer()).
                ExecuteAsync();
        }

        [TestMethod]
        public async Task PutComplexData()
        {
            var postRequest = new Request
            {
                Id = 42,
                Data = "DATA"
            };
            var client = GetClient();
            var response = await client.
                NewRequest(HttpVerb.Put, "PutTest/complex").
                AddContent(postRequest).
                ExecuteAsync<Response>();

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
            client = GetClientXML();
            response = await client.
                NewRequest(HttpVerb.Put, "PutTest/complex").
                AddContent(postRequest).
                ExecuteAsync<Response>();

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
        }
    }
}