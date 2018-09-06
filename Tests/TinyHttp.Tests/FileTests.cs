using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Tiny.Http.Tests
{
    [TestClass]
    public class FileTests : BaseTest
    {
        private const string FileName = "myTextFile.txt";
        private const string Content = "content";

        private const string FileName1 = "myTextFile1.txt";
        private const string FileName2 = "myTextFile2.txt";

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task TestArgumentNullException()
        {
            var client = GetClient();

            var data = await client.
              PostRequest("File/One").
              AddFileContent(null, "text/plain").
              ExecuteAsync<string>();
        }

        [TestMethod]
        public async Task TestDownloadFile()
        {
            var client = GetClient();

            var fileName = System.IO.Path.GetTempFileName().Replace(".tmp", ".pdf");
            var data = await client.
              GetRequest("File/GetPdf").
              DownloadFileAsync(fileName);

            Assert.IsTrue(data.Exists);
            data.Delete();
        }

        [TestMethod]
        public async Task TestDownloadFileStreamEmpty()
        {
            var client = GetClient();

            var fileName = System.IO.Path.GetTempFileName().Replace(".tmp", ".pdf");
            var data = await client.
              GetRequest("File/NoResult").
              DownloadFileAsync(fileName);

            Assert.IsTrue(data.Exists);
            data.Delete();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task TestDownloadFileFilePathNull()
        {
            var client = GetClient();

            var data = await client.
              GetRequest("File/GetPdf").
              DownloadFileAsync(null);
            Assert.IsTrue(data.Exists);
            data.Delete();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task TestMultiPartArgumentNullException()
        {
            var client = GetClient();

            var data = await client.
              PostRequest("File/One").
              AsMultiPartFromDataRequest().
              AddFileContent(null, "text/plain").
              AddFileContent(null, "text/plain").
              ExecuteAsync<string>();
        }

        [ExpectedException(typeof(FileNotFoundException))]
        [TestMethod]
        public async Task TestFileNotFoundException()
        {
            var client = GetClient();

            var fileInfo = new FileInfo("NotFound.txt");
            await client.
              PostRequest("File/One").
              AddFileContent(fileInfo, "text/plain").
              ExecuteAsync();
        }

        [ExpectedException(typeof(FileNotFoundException))]
        [TestMethod]
        public async Task TestFileNotFoundMultipartException()
        {
            var client = GetClient();

            var fileInfo = new FileInfo("NotFound.txt");
            await client.
              PostRequest("File/One").
              AsMultiPartFromDataRequest().
              AddFileContent(fileInfo, "text/plain").
              ExecuteAsync();
        }

        [TestMethod]
        public async Task Send1File()
        {
            var client = GetClient();

            var fileInfo = new FileInfo(FileName);
            var data = await client.
              PostRequest("File/One").
              AddFileContent(fileInfo, "text/plain").
              ExecuteAsync<string>();

            Assert.AreEqual<string>(data, Content);
        }

        [TestMethod]
        public async Task Send2File()
        {
            var client = GetClient();

            var fileInfo1 = new FileInfo(FileName1);
            var fileInfo2 = new FileInfo(FileName2);

            await client.
              PostRequest("File/One").
              AsMultiPartFromDataRequest().
              AddFileContent(fileInfo1, "text/plain").
              AddFileContent(fileInfo2, null, null, "text/plain").
              ExecuteAsync();
        }

        [TestInitialize]
        public async Task InitializeAsync()
        {
            await CreateFileIfNeededAsync(FileName);
            await CreateFileIfNeededAsync(FileName1);
            await CreateFileIfNeededAsync(FileName2);
        }

        private async Task CreateFileIfNeededAsync(string fileName)
        {
            if (!File.Exists(fileName))
            {
                using (var sw = File.CreateText(fileName))
                {
                    await sw.WriteLineAsync(Content);
                    await sw.FlushAsync();
                    sw.Close();
                }
            }
        }

        private void DeleteFileIfNeeded(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            DeleteFileIfNeeded(FileName);
            DeleteFileIfNeeded(FileName1);
            DeleteFileIfNeeded(FileName2);
        }
    }
}