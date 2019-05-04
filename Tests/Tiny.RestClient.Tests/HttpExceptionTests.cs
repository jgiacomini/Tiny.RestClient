using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class HttpExceptionTests : BaseTest
    {
        [TestMethod]
        public async Task CheckIfHttpExceptionReadHeaders()
        {
            bool exceptionThrowed = false;
            var client = GetClient();
            try
            {
                // Call an api not found (the ETag header is present in all responses on this server)
                await client.GetRequest("APIWhichNotExists").ExecuteAsync();
            }
            catch (HttpException ex)
            {
                exceptionThrowed = true;

                Assert.IsTrue(ex.ResponseHeaders.Contains("ETag"), "An header name 'ETag' must be present in response.");
            }

            Assert.IsTrue(exceptionThrowed, $"An {nameof(HttpException)} must be throwed");
        }

        [ExpectedException(typeof(NotFoundCustomException))]
        [TestMethod]
        public async Task CheckIfEnclapsulationWorks()
        {
            var client = GetNewClient();
            client.Settings.EncapsulateHttpExceptionHandler = (ex) =>
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new NotFoundCustomException();
                }

                return ex;
            };

            // Call an API wich throw NotFound error
            await client.GetRequest("APIWhichNotExists").ExecuteAsync();
        }

        internal class NotFoundCustomException : Exception
        {
        }
    }
}
