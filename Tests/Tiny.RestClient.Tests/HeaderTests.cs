using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class HeaderTests : BaseTest
    {
        [TestMethod]
        public async Task CalculateHeadersTest()
        {
            var client = GetNewClient();

            var headers = new Headers
                {
                    { "Header1", "Value1" },
                    { "Header2", "Value2" },
                    { "Header3", "Value3" }
                };

            client.Settings.CalculateHeadersHandler = () =>
            {
                return Task.FromResult(headers);
            };

            await client.
                GetRequest("HeadersTest/NoResponse").
                FillResponseHeaders(out Headers responseHeaders).
                ExecuteAsync();

            foreach (var sendedHeader in headers)
            {
                Assert.IsTrue(responseHeaders.Any(h => h.Key == "FROM_CLIENT" + sendedHeader.Key), $"{sendedHeader.Key} seem not passed to server side");
                var responseHeader = responseHeaders.FirstOrDefault(h => h.Key == "FROM_CLIENT" + sendedHeader.Key);
                Assert.IsTrue(sendedHeader.Value == responseHeader.Value, $"Values for header {sendedHeader.Key} seem not match with values resend by server");
            }
        }
    }
}
