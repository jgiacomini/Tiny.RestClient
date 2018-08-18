using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tiny.Http.Models;

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
            var response = await client.NewRequest(HttpVerb.Post, "PostTest/FromForm").
                AddFormParameter("id", id.ToString()).
                AddFormParameter("data", data).ExecuteAsync<PostResponse>();
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
        }

        [TestMethod]
        public async Task PostWithoutResponse()
        {
            var postRequest = new PostRequest();
            postRequest.Id = 42;
            postRequest.Data = "DATA";

            var client = GetClient();
            await client.NewRequest(HttpVerb.Post, "PostTest/noResponse").
                AddContent(postRequest).
                ExecuteAsync();
        }

        [TestMethod]
        public async Task PostComplexData()
        {
            var postRequest = new PostRequest
            {
                Id = 42,
                Data = "DATA"
            };

            var client = GetClient();
            var response = await client.NewRequest(HttpVerb.Post, "PostTest/complex").
                AddContent(postRequest).
                ExecuteAsync<PostResponse>();

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
        }
    }
}