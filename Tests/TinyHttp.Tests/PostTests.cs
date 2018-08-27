using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tiny.Http.Tests.Models;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class PostTests : BaseTest
    {
        [TestMethod]
        public async Task PostFromFrom()
        {
            int id = 42;
            string data = "DATA";
            var dictionary = new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "data", data }
            };

            var client = GetClient();
            var response = await client.
                PostRequest("PostTest/FromForm").
                AddFormParameter("id", id.ToString()).
                AddFormParameter("data", data).
                ExecuteAsync<Response>();
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
        }

        [TestMethod]
        public async Task PostWithoutResponse()
        {
            var postRequest = new Request();
            postRequest.Id = 42;
            postRequest.Data = "DATA";

            var client = GetClient();
            await client.
                PostRequest("PostTest/noResponse", postRequest).
                ExecuteAsync();
        }

        [TestMethod]
        public async Task PostComplexData()
        {
            var postRequest = new Request
            {
                Id = 42,
                Data = "DATA"
            };

            var client = GetClient();
            var response = await client.
                PostRequest("PostTest/complex", postRequest).
                ExecuteAsync<Response>();

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
        }
    }
}