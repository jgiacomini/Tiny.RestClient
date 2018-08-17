using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Tiny.Http.Models;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class DeleteTests : BaseTest
    {
        [TestMethod]
        public async Task DeleteWithoutResponse()
        {
            var client = GetClient();
            await client.DeleteAsync("DeleteTest/noResponse");

            client = GetClientWithXMLSerialization();
            await client.DeleteAsync("DeleteTest/noResponse");
        }

        [TestMethod]
        public async Task DeleteComplexData()
        {
            var client = GetClient();
            int id = 42;
            string data = "DATA";
            var response = await client.DeleteAsync<PostResponse>($"DeleteTest/complex?id={id}&data={data}");

            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);

            client = GetClientWithXMLSerialization();
            response = await client.DeleteAsync<PostResponse>($"DeleteTest/complex?id={id}&data={data}");
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(data, response.ResponseData);
        }
    }
}