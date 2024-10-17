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
        #region Client scope
        [ExpectedException(typeof(HttpException))]
        [TestMethod]
        public async Task GetErrorWhenCallApiWhenError500()
        {
            var client = GetClient();

            try
            {
                var response = await client.
                    GetRequest("GetTest/Status500Response").
                    ExecuteAsync<IEnumerable<string>>();
            }
            catch (HttpException ex)
            {
                Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, ex.StatusCode);

                throw;
            }
        }

        [TestMethod]
        public async Task GetAnyStatusResponseAllowed()
        {
            var client = GetNewClient();
            client.Settings.HttpStatusCodeAllowed.AllowAnyStatus = true;

            var response = await client.
                GetRequest("GetTest/Status500Response").
                ExecuteAsync<IEnumerable<string>>();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task GetRangeOfStatusesAllowed()
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
        public async Task GetSpecificStatusResponseAllowed()
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
        #endregion

        #region Request scope
        [TestMethod]
        public async Task ForRequest_GetAnyStatusResponseAllowed()
        {
            var client = GetClient();
            var response = await client.
                GetRequest("GetTest/Status500Response").
                AllowAnyHttpStatusCode().
                ExecuteAsync<IEnumerable<string>>();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task ForRequest_GetRangeOfStatusesResponseAllowed()
        {
            var client = GetClient();
            var response = await client.
                GetRequest("GetTest/Status409Response").
                AllowRangeHttpStatusCode(System.Net.HttpStatusCode.BadRequest, System.Net.HttpStatusCode.BadGateway).
                ExecuteAsync<IEnumerable<string>>();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task ForRequest_GetSpecificStatusResponseAllowed()
        {
            var client = GetClient();
            var response = await client.
                GetRequest("GetTest/Status409Response").
                AllowSpecificHttpStatusCode(System.Net.HttpStatusCode.Conflict).
                ExecuteAsync<IEnumerable<string>>();
            Assert.IsNotNull(response);
        }
        #endregion
    }
}
