using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class GetTests : BaseTest
    {
        [TestMethod]
        public async Task GetWithoutResponse()
        {
            var client = GetClient();
            await client.
                GetRequest("GetTest/noResponse").
                ExecuteAsync();
        }

        [TestMethod]
        public async Task GetSimpleData()
        {
            var client = GetClient();
            var data = await client.
                GetRequest("GetTest/simple").
                ExecuteAsync<bool>();
            Assert.AreEqual(data, true);
            client = GetClientXML();
            data = await client.
                GetRequest("GetTest/simple").
                ExecuteAsync<bool>();
            Assert.AreEqual(data, true);
        }

        [TestMethod]
        public async Task GetQueryStringTest()
        {
            var client = GetClient();
            string str = "str";
            int number = 1;
            int? numberNullable = null;
            bool boolean = false;
            bool? boolNullable = null;
            double doubleNumber = 4042.2;
            double? doubleNumberNullable = null;
            decimal decimalNumber = 1;
            decimal? decimalNumberNullable = null;
            float floatNumber = 43.4f;
            float? floatNumberNullable = null;
            var toCompare = $"{str}_{number}_{numberNullable}_{boolean}_{boolNullable}_{doubleNumber}_{doubleNumberNullable}_{decimalNumber}_{decimalNumberNullable}_{floatNumber}_{floatNumberNullable}";
            var data = await client.
                GetRequest("GetTest/QueryString").
                AddQueryParameter("str", str).
                AddQueryParameter("number", number).
                AddQueryParameter("numberNullable", numberNullable).
                AddQueryParameter("bool", boolean).
                AddQueryParameter("boolNullable", boolNullable).
                AddQueryParameter("doubleNumber", doubleNumber).
                AddQueryParameter("doubleNumberNullable", doubleNumberNullable).
                AddQueryParameter("decimalNumber", decimalNumber).
                AddQueryParameter("decimalNumberNullable", decimalNumberNullable).
                AddQueryParameter("floatNumber", floatNumber).
                AddQueryParameter("floatNumberNullable", floatNumberNullable).
                ExecuteAsync<string>();

            Assert.AreEqual(data, toCompare);
            str = null;
            numberNullable = 222;
            boolNullable = false;
            doubleNumberNullable = 44444444;
            decimalNumberNullable = 4445855.2m;
            floatNumberNullable = 64443.2f;
            toCompare = $"{str}_{number}_{numberNullable}_{boolean}_{boolNullable}_{doubleNumber}_{doubleNumberNullable}_{decimalNumber}_{decimalNumberNullable}_{floatNumber}_{floatNumberNullable}";

            data = await client.
                GetRequest("GetTest/QueryString").
                AddQueryParameter("str", str).
                AddQueryParameter("number", number).
                AddQueryParameter("numberNullable", numberNullable).
                AddQueryParameter("bool", boolean).
                AddQueryParameter("boolNullable", boolNullable).
                AddQueryParameter("doubleNumber", doubleNumber).
                AddQueryParameter("doubleNumberNullable", doubleNumberNullable).
                AddQueryParameter("decimalNumber", decimalNumber).
                AddQueryParameter("decimalNumberNullable", decimalNumberNullable).
                AddQueryParameter("floatNumber", floatNumber).
                AddQueryParameter("floatNumberNullable", floatNumberNullable).
                ExecuteAsync<string>();
            Assert.AreEqual(data, toCompare);
        }

        [TestMethod]
        public async Task GetComplexData()
        {
            var client = GetClient();
            var data = await client.GetRequest("GetTest/complex").ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            client = GetClientXML();
            data = await client.GetRequest("GetTest/complex").ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }

        [TestMethod]
        public async Task GetStreamData()
        {
            var client = GetClient();
            var stream = await client.GetRequest("GetTest/stream").WithStreamResponse().ExecuteAsync();
            Assert.AreEqual(stream.Length, 42);
        }
    }
}
