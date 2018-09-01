using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
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
        public async Task GetHeadersOfResponse()
        {
            Headers headersOfResponse = null;
            var client = GetClient();
            await client.
                GetRequest("GetTest/HeadersOfResponse").
                FillResponseHeaders(out headersOfResponse).
                ExecuteAsync();

            Assert.IsTrue(headersOfResponse.Count() == 3);

            for (int i = 0; i < headersOfResponse.Count(); i++)
            {
                var current = headersOfResponse.ElementAt(i);

                Assert.IsTrue(current.Key == $"custom{i + 1}");
            }
        }

        [TestMethod]
        public async Task GetWithCustomHeaders()
        {
            var client = GetClient();
            var result = await client.
                GetRequest("GetTest/WithHeaders").
                AddHeader("header1", "headerContent1").
                AddHeader("header2", "headerContent2").
                AddHeader("header3", "headerContent3").
                ExecuteAsync<List<string>>();

            Assert.IsTrue(result.Count == 3);

            for (int i = 0; i < result.Count; i++)
            {
                var current = result[i];
                var currentId = i + 1;
                Assert.IsTrue(current == $"header{currentId}_headerContent{currentId}");
            }
        }

        [TestMethod]
        public async Task GetQueryStringTest()
        {
            var client = GetClient();
            string str = "str";
            int number = 1;
            int? numberNullable = null;
            uint numberUnsigned = 1;
            uint? numberUnsignedNullable = null;
            bool boolean = false;
            bool? boolNullable = null;
            double doubleNumber = 4042.2;
            double? doubleNumberNullable = null;
            decimal decimalNumber = 1;
            decimal? decimalNumberNullable = null;
            float floatNumber = 43.4f;
            float? floatNumberNullable = null;
            var toCompare = $"{str}_{number}_{numberNullable}_{numberUnsigned}_{numberUnsignedNullable}_{boolean}_{boolNullable}_{doubleNumber}_{doubleNumberNullable}_{decimalNumber}_{decimalNumberNullable}_{floatNumber}_{floatNumberNullable}";
            var data = await client.
                GetRequest("GetTest/QueryString").
                AddQueryParameter("str", str).
                AddQueryParameter("number", number).
                AddQueryParameter("numberNullable", numberNullable).
                AddQueryParameter("numberUnsigned", numberUnsigned).
                AddQueryParameter("numberUnsignedNullable", numberUnsignedNullable).
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
            numberUnsignedNullable = 333;
            boolNullable = false;
            doubleNumberNullable = 44444444;
            decimalNumberNullable = 4445855.2m;
            floatNumberNullable = 64443.2f;
            toCompare = $"{str}_{number}_{numberNullable}_{numberUnsigned}_{numberUnsignedNullable}_{boolean}_{boolNullable}_{doubleNumber}_{doubleNumberNullable}_{decimalNumber}_{decimalNumberNullable}_{floatNumber}_{floatNumberNullable}";

            data = await client.
                GetRequest("GetTest/QueryString").
                AddQueryParameter("str", str).
                AddQueryParameter("number", number).
                AddQueryParameter("numberNullable", numberNullable).
                AddQueryParameter("numberUnsigned", numberUnsigned).
                AddQueryParameter("numberUnsignedNullable", numberUnsignedNullable).
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
