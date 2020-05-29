using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using Tiny.RestClient.Tests.Models;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class MultipartTests : BaseTest
    {
        [TestMethod]
        public async Task SendMultipleData()
        {
            var postRequest = new Request
            {
                Id = 42,
                Data = "DATA"
            };
            var client = GetClient();

            var data = await client.
              PostRequest("MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddContent<Request>(postRequest, "request", "request.json").
              AddByteArray(new byte[42], "bytesArray", "bytesArray.bin").
              AddStream(new MemoryStream(new byte[42]), "stream", "stream.bin").
              AddString("string", "string", "string.txt").
              ExecuteAsync<string>();

            Assert.AreEqual<string>("request-request.json;bytesArray-bytesArray.bin;stream-stream.bin;string-string.txt;", data);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task MultiPartAddStreamNull()
        {
            var client = GetClient();

            var data = await client.
              PostRequest("MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddStream(null).
              ExecuteAsync<string>();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task MultiPartAddByteArrayNull()
        {
            var client = GetClient();

            await client.
              PostRequest("MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddByteArray(null, "bytesArray", "bytesArray.bin").
              ExecuteAsync<string>();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task MultiPartAddContentNull()
        {
            var client = GetClient();

            await client.
              PostRequest("MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddContent<Request>(null).
              ExecuteAsync<string>();
        }
    }
}