﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class ETagTests : BaseTest
    {
        private string _directoryPath;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            var tempPath = System.IO.Path.GetTempPath();
            _directoryPath = Path.Combine(tempPath, $"{nameof(ETagTests)}_{TestContext.TestName}");

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
        public async Task ETagContainerOnClient()
        {
            var client = GetNewClient();

            var etagContainer = new ETagFileContainer(_directoryPath);
            client.Settings.ETagContainer = etagContainer;
            var data = await client.GetRequest("GetTest/complex").
                FillResponseHeaders(out Headers headers).
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            var actionUri = new Uri($"{ServerUrl}GetTest/complex");
            var etag = headers["ETag"].FirstOrDefault();
            var etagStored = await etagContainer.GetExistingETagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            var fakeData = new List<string>() { "test1", "test2" };

            var json = await client.Settings.Formatters.FirstOrDefault().SerializeAsync<List<string>>(fakeData, client.Settings.Encoding, CancellationToken.None);
            await etagContainer.SaveDataAsync(actionUri, etagStored, new MemoryStream(Encoding.UTF8.GetBytes(json)), CancellationToken.None);

            data = await client.GetRequest("GetTest/complex").
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "test1");
            Assert.AreEqual(data[1], "test2");

            await etagContainer.SaveDataAsync(actionUri, "\"TEST\"", new MemoryStream(), CancellationToken.None);

            data = await client.GetRequest("GetTest/complex").
               ExecuteAsync<string[]>();

            etagStored = await etagContainer.GetExistingETagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }

        [TestMethod]
        public async Task ETagContainerOnRequest()
        {
            var client = GetNewClient();

            var etagContainer = new ETagFileContainer(_directoryPath);
            var data = await client.GetRequest("GetTest/complex").
                WithETagContainer(etagContainer).
                FillResponseHeaders(out Headers headers).
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");

            var actionUri = new Uri($"{ServerUrl}GetTest/complex");
            var etag = headers["ETag"].FirstOrDefault();
            var etagStored = await etagContainer.GetExistingETagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            var fakeData = new List<string>() { "test1", "test2" };

            var json = await client.Settings.Formatters.FirstOrDefault().SerializeAsync<List<string>>(fakeData, client.Settings.Encoding, CancellationToken.None);
            await etagContainer.SaveDataAsync(actionUri, etagStored, new MemoryStream(Encoding.UTF8.GetBytes(json)), CancellationToken.None);

            data = await client.GetRequest("GetTest/complex").
                WithETagContainer(etagContainer).
                ExecuteAsync<string[]>();
            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "test1");
            Assert.AreEqual(data[1], "test2");

            await etagContainer.SaveDataAsync(actionUri, "\"TEST\"", new MemoryStream(), CancellationToken.None);

            data = await client.GetRequest("GetTest/complex").
                WithETagContainer(etagContainer).
                ExecuteAsync<string[]>();

            etagStored = await etagContainer.GetExistingETagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            Assert.AreEqual(data.Length, 2);
            Assert.AreEqual(data[0], "value1");
            Assert.AreEqual(data[1], "value2");
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void ETagFileContainerDirectoryNotFound()
        {
            new ETagFileContainer(@"C:\notfound");
            Assert.Fail("It must not go here");
        }
    }
}
