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

        public T Deserialize<T>(Stream stream, Encoding encoding)
        {
            throw new System.NotImplementedException();
        }

        public string Serialize<T>(T data, Encoding encoding)
            where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
