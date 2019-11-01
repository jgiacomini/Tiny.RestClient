using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class StatusRangeTests : BaseTest
    {
        [TestMethod]
        public async Task GetStatus500ResponseAllowed()
        {
            var client = GetNewClient();
            client.Settings.HttpStatusCodeAllowed.AllowAnyStatus = true;

            var response = await client.
                GetRequest("GetTest/Status500Response").
                ExecuteAsync<IEnumerable<string>>();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task GetStatus409ResponseAllowedMultiStatusesAllowed()
        {
            var client = GetNewClient();
            client.Settings.HttpStatusCodeAllowed.Add(
                new HttpStatusRange(
                    System.Net.HttpStatusCode.BadRequest, // 400
                    System.Net.HttpStatusCode.BadGateway)); // 502
            var response = await client.
                GetRequest("GetTest/Status409Response").
                ExecuteAsync<IEnumerable<string>>();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task GetStatus409ResponseAllowed()
        {
            var client = GetNewClient();
            client.Settings.HttpStatusCodeAllowed.Add(new HttpStatusRange(System.Net.HttpStatusCode.Conflict));
            var response = await client.
                GetRequest("GetTest/Status409Response").
                ExecuteAsync<IEnumerable<string>>();
            Assert.IsNotNull(response);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AddInvalidStatusRange()
        {
            var client = GetNewClient();
            client.Settings.HttpStatusCodeAllowed.Add(new HttpStatusRange(500, 400));
        }
    }
}
