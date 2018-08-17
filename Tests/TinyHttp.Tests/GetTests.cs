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
            var fluentClient = GetClient();
            await fluentClient.NewRequest(HttpVerb.Get, "GetTest/noResponse").ExecuteAsync();
        }

        [TestMethod]
        public async Task GetSimpleData()
        {
            var fluentClient = GetClient();
            var data = await fluentClient.NewRequest(HttpVerb.Get, "GetTest/simple").ExecuteAsync<bool>();
            Assert.AreEqual(data, true);
            fluentClient = GetClientXML();
            data = await fluentClient.NewRequest(HttpVerb.Get, "GetTest/simple").ExecuteAsync<bool>();
            Assert.AreEqual(data, true);
        }

        [TestMethod]
        public async Task GetComplexData()
        {
            var fluentClient = GetClient();
            var data = await fluentClient.NewRequest(HttpVerb.Get, "GetTest/complex").ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            fluentClient = GetClientXML();
            data = await fluentClient.NewRequest(HttpVerb.Get, "GetTest/complex").ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }

        [TestMethod]
        public async Task GetStreamData()
        {
            var fluentClient = GetClientXML();
            var stream = await fluentClient.NewRequest(HttpVerb.Get, "GetTest/stream").WithStreamResponse().ExecuteAsync();
            Assert.AreEqual(stream.Length, 42);
        }
    }
}
