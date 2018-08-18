using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class GetTests : BaseTest
    {
        [TestMethod]
        public async Task GetWithoutResponse()
        {
            var client = GetClient();
            await client.NewRequest(HttpVerb.Get, "GetTest/noResponse").ExecuteAsync();
        }

        [TestMethod]
        public async Task GetSimpleData()
        {
            var client = GetClient();
            var data = await client.NewRequest(HttpVerb.Get, "GetTest/simple").ExecuteAsync<bool>();
            Assert.AreEqual(data, true);
            client = GetClientXML();
            data = await client.NewRequest(HttpVerb.Get, "GetTest/simple").ExecuteAsync<bool>();
            Assert.AreEqual(data, true);
        }

        [TestMethod]
        public async Task GetComplexData()
        {
            var client = GetClient();
            var data = await client.NewRequest(HttpVerb.Get, "GetTest/complex").ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            client = GetClientXML();
            data = await client.NewRequest(HttpVerb.Get, "GetTest/complex").ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }

        [TestMethod]
        public async Task GetStreamData()
        {
            var client = GetClientXML();
            var stream = await client.NewRequest(HttpVerb.Get, "GetTest/stream").WithStreamResponse().ExecuteAsync();
            Assert.AreEqual(stream.Length, 42);
        }
    }
}
