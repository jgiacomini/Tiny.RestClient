using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
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
                await client.GetRequest("HeaderTest/Get").ExecuteAsync();
            }
            catch (HttpException ex)
            {
                exceptionThrowed = true;

                Assert.IsTrue(ex.ResponseHeaders.Contains("CustomHeader"), "An header name 'CustomHeader' must be present in response");
            }

            Assert.IsTrue(exceptionThrowed, $"An {nameof(HttpException)} must be throwed");
        }
    }
}
