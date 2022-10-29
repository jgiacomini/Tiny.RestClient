using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tiny.RestClient.ForTest.Api.Models;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class CaseTests : BaseTest
    {
        [TestMethod]
        public async Task KebabTest()
        {
            var client = GetNewClient();
            client.Settings.Formatters.OfType<JsonFormatter>().First().UseKebabCase();

            var rep = await client.
                GetRequest("case/Kebab").
                ExecuteAsync<Response>();

            Assert.AreEqual(rep.Id, 42);
            Assert.AreEqual(rep.ResponseData, "REP");
        }

        [TestMethod]
        public async Task CamelTest()
        {
            var client = GetNewClient();
            client.Settings.Formatters.OfType<JsonFormatter>().First().UseCamelCase();

            var rep = await client.
                GetRequest("case/Camel").
                ExecuteAsync<Response>();
            Assert.AreEqual(rep.Id, 42);
            Assert.AreEqual(rep.ResponseData, "REP");
        }

        [TestMethod]
        public async Task SnakeTest()
        {
            var client = GetNewClient();
            client.Settings.Formatters.OfType<JsonFormatter>().First().UseSnakeCase();

            var rep = await client.
                GetRequest("case/Snake").
                ExecuteAsync<Response>();
            Assert.AreEqual(rep.Id, 42);
            Assert.AreEqual(rep.ResponseData, "REP");
        }
    }
}
