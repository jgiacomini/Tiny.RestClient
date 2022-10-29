using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tiny.RestClient.ForTest.Api.Models;
using Tiny.RestClient.Tests.Utils;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class SerializeExceptionTests : BaseTest
    {
        [ExpectedException(typeof(SerializeException))]
        [TestMethod]
        public async Task SerializeExceptionTestAsync()
        {
            var client = GetClient();
            await client.
                PostRequest().
                AddContent<Request>(new Request(), new ExceptionFormatter()).
                ExecuteAsync();
        }
    }
}
