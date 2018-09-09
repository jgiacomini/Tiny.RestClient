using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tiny.RestClient.Tests.Models;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class PostTests : BaseTest
    {
        [TestMethod]
        public async Task PostFromFrom()
        {
            int id = 42;
            string data = "DATA";

            var client = GetClient();
            var response = await client.
                PostRequest("PostTest/FromForm").
                AddFormParameter("id", id.ToString()).
                AddFormParameter("data", data).
                ExecuteAsync<Response>();
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);

            response = await client.
            PostRequest("PostTest/FromForm").
            AddFormParameters(new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "data", data }
            }).
            ExecuteAsync<Response>();

            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
        }

        [TestMethod]
        public async Task PostWithoutResponse()
        {
            var postRequest = new Request
            {
                Id = 42,
                Data = "DATA"
            };

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

            response = await client.
                PostRequest("PostTest/complex", postRequest, new XmlFormatter()).
                ExecuteAsync<Response>();

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);

            response = await client.
                PostRequest("PostTest/complex", postRequest).
                ExecuteAsync<Response>(new JsonFormatter());

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);

            client = GetClientForUrl(_serverUrl + "PostTest/complex");
            response = await client.
                PostRequest(postRequest).
                ExecuteAsync<Response>();

            Assert.AreEqual(postRequest.Id, response.Id);
            Assert.AreEqual(postRequest.Data, response.ResponseData);
        }

        [TestMethod]
        public async Task PostByteArrayData()
        {
            uint size = 2048;
            var client = GetClient();

            var byteArray = GetByteArray(size);

            var response = await client.
                 PostRequest("PostTest/Stream").
                AddByteArrayContent(GetByteArray(size)).
                ExecuteAsByteArrayAsync();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Length == size);

            for (int i = 0; i < byteArray.Length; i++)
            {
                Assert.IsTrue(byteArray[i] == response[i], "byte array response must have same data than byte array sended");
            }
        }

        [TestMethod]
        public async Task PostStreamData()
        {
            uint size = 4024;
            var client = GetClient();
            var response = await client.
                PostRequest("PostTest/Stream").
                AddStreamContent(new MemoryStream(GetByteArray(size))).
                ExecuteAsStreamAsync();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Length == size);
        }
    }
}