#if !FILEINFO_NOT_SUPPORTED
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Class <see cref="EtagFileContainer"/> which store data of entity in a directory
    /// </summary>
    public class EtagFileContainer : IEtagContainer
    {
        private const int BufferSize = 81920;
        private readonly string _pathOfDirectoryContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EtagFileContainer"/> class.
        /// </summary>
        /// <param name="pathOfDirectoryContainer">the path of the directory which will store the data</param>
        public EtagFileContainer(string pathOfDirectoryContainer)
        {
            _pathOfDirectoryContainer = pathOfDirectoryContainer ?? throw new ArgumentNullException(nameof(pathOfDirectoryContainer));

            if (!Directory.Exists(_pathOfDirectoryContainer))
            {
                throw new DirectoryNotFoundException($"Directory '{_pathOfDirectoryContainer}' not found");
            }
        }

        /// <inheritdoc/>
        public Task<string> GetExistingEtagAsync(Uri uri, CancellationToken cancellationToken)
        {
            var url = uri.AbsoluteUri;
            var key = CalculateMD5Hash(url);
            var hashPath = GetEtagPath(key);
            if (File.Exists(hashPath))
            {
                return Task.FromResult(File.ReadAllText(hashPath));
            }

            return Task.FromResult<string>(null);
        }

        /// <inheritdoc/>
        public Task<Stream> GetDataAsync(Uri uri, CancellationToken cancellationToken)
        {
            var url = uri.AbsoluteUri;
            var key = CalculateMD5Hash(url);
            var dataPath = GetDataPath(key);
            return Task.FromResult((Stream)File.OpenRead(dataPath));
        }

        /// <inheritdoc/>
        public async Task SaveDataAsync(Uri uri, string etag, Stream stream, CancellationToken cancellationToken)
        {
            var url = uri.AbsoluteUri;
            var key = CalculateMD5Hash(url);

            var hashPath = GetEtagPath(key);
            var dataPath = GetDataPath(key);

            if (File.Exists(hashPath))
            {
                File.Delete(hashPath);
            }

            if (File.Exists(dataPath))
            {
                File.Delete(dataPath);
            }

            using (var fileStream = File.Create(dataPath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileStream, BufferSize, cancellationToken).ConfigureAwait(false);
            }

            var buffer = Encoding.ASCII.GetBytes(etag);
            using (var fs = new FileStream(hashPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                await fs.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
            }
        }

        private string GetEtagPath(string key)
        {
            return Path.Combine(_pathOfDirectoryContainer, $"{key}.etag");
        }

        private string GetDataPath(string key)
        {
            return Path.Combine(_pathOfDirectoryContainer, key);
        }

        private string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
#endif