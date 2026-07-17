using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual(2, data.Length);
            Assert.AreEqual("value1", data[0]);
            Assert.AreEqual("value2", data[1]);

            var actionUri = new Uri($"{ServerUrl}GetTest/complex");
            var etag = headers["ETag"].FirstOrDefault();
            var etagStored = await etagContainer.GetExistingETagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            var fakeData = new List<string>() { "test1", "test2" };

            var json = await client.Settings.Formatters.FirstOrDefault().SerializeAsync<List<string>>(fakeData, client.Settings.Encoding, CancellationToken.None);
            await etagContainer.SaveDataAsync(actionUri, etagStored, new MemoryStream(Encoding.UTF8.GetBytes(json)), CancellationToken.None);

            data = await client.GetRequest("GetTest/complex").
                ExecuteAsync<string[]>();
            Assert.AreEqual(2, data.Length);
            Assert.AreEqual("test1", data[0]);
            Assert.AreEqual("test2", data[1]);

            await etagContainer.SaveDataAsync(actionUri, "\"TEST\"", new MemoryStream(), CancellationToken.None);

            data = await client.GetRequest("GetTest/complex").
               ExecuteAsync<string[]>();

            etagStored = await etagContainer.GetExistingETagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            Assert.AreEqual(2, data.Length);
            Assert.AreEqual("value1", data[0]);
            Assert.AreEqual("value2", data[1]);
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
            Assert.AreEqual(2, data.Length);
            Assert.AreEqual("value1", data[0]);
            Assert.AreEqual("value2", data[1]);

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
            Assert.AreEqual(2, data.Length);
            Assert.AreEqual("test1", data[0]);
            Assert.AreEqual("test2", data[1]);

            await etagContainer.SaveDataAsync(actionUri, "\"TEST\"", new MemoryStream(), CancellationToken.None);

            data = await client.GetRequest("GetTest/complex").
                WithETagContainer(etagContainer).
                ExecuteAsync<string[]>();

            etagStored = await etagContainer.GetExistingETagAsync(actionUri, CancellationToken.None);
            Assert.AreEqual(etagStored, etag);

            Assert.AreEqual(2, data.Length);
            Assert.AreEqual("value1", data[0]);
            Assert.AreEqual("value2", data[1]);
        }

        [TestMethod]
        public void ETagFileContainerDirectoryNotFound()
        {
            Assert.ThrowsExactly<DirectoryNotFoundException>(() =>
                new ETagFileContainer(@"C:\notfound"));
        }
    }
}
