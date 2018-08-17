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
            var client = GetClient();

            int id = 42;
            string data = "DATA";
            var dictionary = new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "data", data }
            };

            var response = await client.PostAsync<PostResponse>("PostTest/FromForm", dictionary);
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
        }

        [TestMethod]
        public async Task PostWithoutResponse()
        {
            var client = GetClient();
            var postRequest = new PostRequest();
            postRequest.Id = 42;
            postRequest.Data = "DATA";
            await client.PostAsync("PostTest/noResponse", postRequest);

            client = GetClientWithXMLSerialization();
            await client.PostAsync("PostTest/noResponse", postRequest);
        }

        [TestMethod]
        public async Task PostComplexData()
        {
            var client = GetClient();
            var postRequest = new PostRequest();
            postRequest.Id = 42;
            postRequest.Data = "DATA";
            var response = await client.PostAsync<PostResponse, PostRequest>("PostTest/complex", postRequest);

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);

            client = GetClientWithXMLSerialization();
            response = await client.PostAsync<PostResponse, PostRequest>("PostTest/complex", postRequest);
            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
        }
    }
}