using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;
using Tiny.RestClient.Tests.Utils;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class DeserializeExceptionTests : BaseTest
    {
        [TestMethod]
        public async Task DeserializeExceptionTestAsync()
        {
            var client = GetClient();

            var ex = await Assert.ThrowsExactlyAsync<DeserializeException>(async () =>
                await client.
                    GetRequest("GetTest/complex").
                    ExecuteAsync<string[]>(new ExceptionFormatter()));

            Debug.WriteLine(ex.DataToDeserialize);
        }
    }
}
