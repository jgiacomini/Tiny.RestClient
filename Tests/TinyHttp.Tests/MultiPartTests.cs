using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            await client.
              NewRequest(HttpVerb.Post, "MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddByteArray(new byte[42], "bytesArray", "bytesArray.bin").
              AddStream(new MemoryStream(), "stream", "stream.bin").
              AddContent<Request>(postRequest, "request", "request.json").
              ExecuteAsync<string>();
        }
    }
}