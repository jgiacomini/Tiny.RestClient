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
            await client.GetAsync("GetTest/noResponse");
        }

        [TestMethod]
        public async Task GetSimpleData()
        {
            var client = GetClient();
            var data = await client.GetAsync<bool>("GetTest/simple");

            Assert.AreEqual(data, true);
            client = GetClientWithXMLSerialization();
            data = await client.GetAsync<bool>("GetTest/simple");
            Assert.AreEqual(data, true);
        }

        [TestMethod]
        public async Task GetComplexData()
        {
            var client = GetClient();
            var data = await client.GetAsync<string[]>("GetTest/complex");

            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            client = GetClientWithXMLSerialization();
            data = await client.GetAsync<string[]>("GetTest/complex");
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }

        [TestMethod]
        public async Task GetStreamData()
        {
            var client = GetClient();
            var stream = await client.GetStreamAsync("GetTest/stream");
            Assert.AreEqual(stream.Length, 42);
        }
    }
}
