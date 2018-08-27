using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
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
            var request = new Request
            {
                Id = 42,
                Data = "DATA"
            };

            var client = GetClient();
            await client.
               PutRequest("PutTest/noResponse", request).
               ExecuteAsync();

            await client.
                PutRequest("PutTest/noResponse", request, new XmlFormatter()).
                ExecuteAsync();
        }

        [TestMethod]
        public async Task PutComplexData()
        {
            var request = new Request
            {
                Id = 42,
                Data = "DATA"
            };
            var client = GetClient();
            var response = await client.
                PutRequest("PutTest/complex", request).
                ExecuteAsync<Response>();

            Assert.AreEqual(request.Id, response.Id);
            Assert.AreEqual(request.Data, response.ResponseData);
            client = GetClientXML();
            response = await client.
                PutRequest("PutTest/complex", request).
                ExecuteAsync<Response>();

            Assert.AreEqual(request.Id, response.Id);
            Assert.AreEqual(request.Data, response.ResponseData);
        }
    }
}