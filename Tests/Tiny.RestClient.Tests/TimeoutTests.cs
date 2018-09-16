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
            var client = GetClient();
            client.Settings.Listeners.AddDebug();
            client.Settings.DefaultTimeout = TimeSpan.FromSeconds(1);
            var data = await client.
              GetRequest("TimeoutTest/Action2Secs").
              ExecuteAsync<string>();

            Debug.WriteLine(data);
        }

        [ExpectedException(typeof(TaskCanceledException))]
        [TestMethod]
        public async Task TimeoutCancelledByUserTest()
        {
            var client = GetClient();
            client.Settings.Listeners.AddDebug();
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
