using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;
using Tiny.RestClient.Tests.Utils;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class DeserializeExceptionTests : BaseTest
    {
        [ExpectedException(typeof(DeserializeException))]
        [TestMethod]
        public async Task DeserializeExceptionTestAsync()
        {
            try
            {
                var client = GetClient();

                var data = await client.
                    GetRequest("GetTest/complex").
                    ExecuteAsync<string[]>(new ExceptionFormatter());
            }
            catch (DeserializeException ex)
            {
                Debug.WriteLine(ex.DataToDeserialize);
                throw;
            }
        }
    }
}
