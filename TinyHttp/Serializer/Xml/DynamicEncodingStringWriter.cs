using System.IO;
using System.Text;

namespace Tiny.Http
{
    internal class DynamicEncodingStringWriter : StringWriter
    {
        private readonly Encoding _encoding;
        public DynamicEncodingStringWriter(Encoding encoding)
        {
            _encoding = encoding;
        }

        public override Encoding Encoding { get { return _encoding; } }
    }
}
