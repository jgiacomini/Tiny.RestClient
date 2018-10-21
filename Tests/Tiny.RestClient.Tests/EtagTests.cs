using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            _directoryPath = Path.Combine(tempPath, TestContext.TestName);

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
        public async Task GetComplexData()
        {
            var client = GetNewClient();
            client.Settings.EtagContainer = new EtagFileContainer(_directoryPath);
            var data = await client.GetRequest("GetTest/complex").
                AddHeader("xxx-TestEtag", "test").
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            data = await client.GetRequest("GetTest/complex").
                AddHeader("xxx-TestEtag", "test").
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }
    }
}
