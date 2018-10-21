using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class EtagTests : BaseTest
    {
        private string _directoryPath;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            var tempPath = System.IO.Path.GetTempPath();
            _directoryPath = Path.Combine(tempPath, $"{nameof(EtagTests)}_{TestContext.TestName}");

            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
            else
            {
                var files = Directory.GetFiles(_directoryPath);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(_directoryPath))
            {
                Directory.Delete(_directoryPath, true);
            }
        }

        [TestMethod]
        public async Task CheckIfETagIsUsed()
        {
            var client = GetNewClient();

            var etagContainer = new EtagFileContainer(_directoryPath);
            client.Settings.EtagContainer = etagContainer;
            var data = await client.GetRequest("GetTest/complex").
                FillResponseHeaders(out Headers headers).
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            var actionUri = new Uri($"{ServerUrl}GetTest/complex");
            var etag = headers["ETag"].FirstOrDefault();
            var etagStored = await etagContainer.GetExistingEtagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            data = await client.GetRequest("GetTest/complex").
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            await etagContainer.SaveDataAsync(actionUri, "\"TEST\"", new MemoryStream(), CancellationToken.None);

            data = await client.GetRequest("GetTest/complex").
               ExecuteAsync<string[]>();

            etagStored = await etagContainer.GetExistingEtagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }
    }
}
