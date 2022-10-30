using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tiny.RestClient.Tests.Utils
{
    internal class ExceptionFormatter : IFormatter
    {
        public string DefaultMediaType => string.Empty;

        public IEnumerable<string> SupportedMediaTypes
        {
            get
            {
                yield return string.Empty;
            }
        }

        public ValueTask<T> DeserializeAsync<T>(Stream stream, Encoding encoding, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> SerializeAsync<T>(T data, Encoding encoding, CancellationToken cancellationToken)
            where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
