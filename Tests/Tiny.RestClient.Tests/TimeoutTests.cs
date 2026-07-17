using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class TimeoutTests : BaseTest
    {
        [TestMethod]
        public async Task TimeoutTest()
        {
            var client = GetNewClient();
            client.Settings.DefaultTimeout = TimeSpan.FromSeconds(1);
            await Assert.ThrowsExactlyAsync<TimeoutException>(async () =>
                await client.
                  GetRequest("TimeoutTest/Action2Secs").
                  ExecuteAsync<string>());
        }

        [TestMethod]
        public async Task LocalTimeoutTest()
        {
            var client = GetClient();
            await Assert.ThrowsExactlyAsync<TimeoutException>(async () =>
                await client.
                  GetRequest("TimeoutTest/Action2Secs").
                  WithTimeout(TimeSpan.FromSeconds(1)).
                  ExecuteAsync<string>());
        }

        [TestMethod]
        public async Task LocalTimeoutCancelledByUserTest()
        {
            var client = GetClient();
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
            {
                await Assert.ThrowsExactlyAsync<TaskCanceledException>(async () =>
                    await client.
                      GetRequest("TimeoutTest/Action2Secs").
                       WithTimeout(TimeSpan.FromSeconds(2)).
                      ExecuteAsync<string>(cts.Token));
            }
        }

        [TestMethod]
        public async Task TimeoutCancelledByUserTest()
        {
            var client = GetClient();
            client.Settings.DefaultTimeout = TimeSpan.FromSeconds(2);

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
            {
                await Assert.ThrowsExactlyAsync<TaskCanceledException>(async () =>
                    await client.
                      GetRequest("TimeoutTest/Action2Secs").
                      ExecuteAsync<string>(cts.Token));
            }
        }
    }
}
