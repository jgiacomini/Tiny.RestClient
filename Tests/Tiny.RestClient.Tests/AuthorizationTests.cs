using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class AuthorizationTests : BaseTest
    {
        [TestMethod]
        public async Task BasicAuthorizationWithUnAuthorizedTestAsync()
        {
            bool isThrowed = false;
            try
            {
                var client = GetClient();
                await client.
                    GetRequest("Authorization/BasicAuthentication").
                    ExecuteAsync();
            }
            catch (HttpException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                isThrowed = true;
            }

            Assert.IsTrue(isThrowed, $"The API must throw exception with StatusCode : {HttpStatusCode.Unauthorized}");
        }

        [TestMethod]
        public async Task BasicAuthorizationTestAsync()
        {
            var client = GetClient();
            await client.
                GetRequest("Authorization/BasicAuthentication").
                WithBasicAuthentication("username", "42").
                ExecuteAsync();
        }

        [TestMethod]
        public async Task BasicAuthorizationWithDefaultHeadersTestAsync()
        {
            var client = GetNewClient();
            client.Settings.DefaultHeaders.AddBasicAuthentication("username", "42");
            await client.
                GetRequest("Authorization/BasicAuthentication").
                ExecuteAsync();
        }

        [TestMethod]
        public async Task BearerAuthorizationWithUnAuthorizedTestAsync()
        {
            bool isThrowed = false;
            try
            {
                var client = GetClient();
                await client.
                    GetRequest("Authorization/BearerAuthentication").
                    ExecuteAsync();
            }
            catch (HttpException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                isThrowed = true;
            }

            Assert.IsTrue(isThrowed, $"The API must throw exception with StatusCode : {HttpStatusCode.Unauthorized}");
        }

        [TestMethod]
        public async Task BearerAuthorizationTestAsync()
        {
            var client = GetClient();
            await client.
                GetRequest("Authorization/BearerAuthentication").
                WithOAuthBearer(Guid.Empty.ToString()).
                ExecuteAsync();
        }

        [TestMethod]
        public async Task BearerAuthorizationWithDefaultHeadersTestAsync()
        {
            var client = GetNewClient();
            client.Settings.DefaultHeaders.AddBearer(Guid.Empty.ToString());
            await client.
                GetRequest("Authorization/BearerAuthentication").
                ExecuteAsync();
        }
    }
}
