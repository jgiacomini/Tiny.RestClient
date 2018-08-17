using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tiny.Http.Models;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class PutTests : BaseTest
    {
        [TestMethod]
        public async Task PutWithoutResponse()
        {
            var client = GetClient();
            var postRequest = new PostRequest();
            postRequest.Id = 42;
            postRequest.Data = "DATA";
            await client.PutAsync("PutTest/noResponse", postRequest);
        }

        [TestMethod]
        public async Task PutComplexData()
        {
            var client = GetClient();
            var postRequest = new PostRequest();
            postRequest.Id = 42;
            postRequest.Data = "DATA";
            var response = await client.PutAsync<PostResponse, PostRequest>("PutTest/complex", postRequest);

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
        }
    }
}