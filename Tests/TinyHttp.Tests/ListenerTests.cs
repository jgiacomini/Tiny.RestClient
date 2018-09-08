using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class ListenerTests : BaseTest
    {
        [TestMethod]
        public async Task AddDebugListener()
        {
            var client = GetClient();
            client.Settings.Listeners.AddDebug();
            client.Settings.Listeners.AddDebug(false);
            await client.
                NewRequest(new System.Net.Http.HttpMethod("GET"), "GetTest/noResponse").
                ExecuteAsync();
        }
    }
}
