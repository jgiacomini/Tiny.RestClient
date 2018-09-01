using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using Tiny.Http.Tests.Models;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class MultiPartTests : BaseTest
    {
        [TestMethod]
        public async Task SendMultipleFile()
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
              AddByteArray(new byte[42], "bytesArray", "bytesArray.bin").
              AddStream(new MemoryStream(new byte[42]), "stream", "stream.bin").
              AddContent<Request>(postRequest, "request", "request.json").
              ExecuteAsync<string>();

            Assert.AreEqual<string>(data, "bytesArray-bytesArray.bin;stream-stream.bin;request-request.json;");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task MultiPartAddStreamNull()
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
              AddByteArray(null, "bytesArray", "bytesArray.bin").
              ExecuteAsync<string>();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task MultiPartAddByteArrayNull()
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
              AddStream(null).
              ExecuteAsync<string>();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task MultiPartAddContentNull()
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
              AddContent<Request>(null).
              ExecuteAsync<string>();
        }
    }
}