using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class TimeoutTests : BaseTest
    {
        [ExpectedException(typeof(TimeoutException))]
        [TestMethod]
        public async Task TimeoutTest()
        {
            var client = GetNewClient();
            client.Settings.DefaultTimeout = TimeSpan.FromSeconds(1);
            var data = await client.
              GetRequest("TimeoutTest/Action2Secs").
              ExecuteAsync<string>();

            Debug.WriteLine(data);
        }

        [ExpectedException(typeof(TimeoutException))]
        [TestMethod]
        public async Task LocalTimeoutTest()
        {
            var client = GetClient();
            var data = await client.
              GetRequest("TimeoutTest/Action2Secs").
              WithTimeout(TimeSpan.FromSeconds(1)).
              ExecuteAsync<string>();

            Debug.WriteLine(data);
        }

        [ExpectedException(typeof(OperationCanceledException))]
        [TestMethod]
        public async Task LocalTimeoutCancelledByUserTest()
        {
            var client = GetClient();
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
            {
                var data = await client.
                  GetRequest("TimeoutTest/Action2Secs").
                   WithTimeout(TimeSpan.FromSeconds(2)).
                  ExecuteAsync<string>(cts.Token);

                Debug.WriteLine(data);
            }
        }

        [ExpectedException(typeof(OperationCanceledException))]
        [TestMethod]
        public async Task TimeoutCancelledByUserTest()
        {
            var client = GetClient();
            client.Settings.DefaultTimeout = TimeSpan.FromSeconds(2);

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
            {
                var data = await client.
                  GetRequest("TimeoutTest/Action2Secs").
                  ExecuteAsync<string>(cts.Token);

                Debug.WriteLine(data);
            }
        }
    }
}
