using System.Linq;
namespace Tiny.Http.Tests
{
    public class BaseTest
    {
        private readonly object _toLock = new object();
        private TinyHttpClient _client;
        private TinyHttpClient _clientXML;
        protected static readonly string _serverUrl = "http://localhost:4242/api/";

        protected TinyHttpClient GetClientForUrl(string url)
        {
            return new TinyHttpClient(Program.Client, url);
        }

        protected TinyHttpClient GetClient()
        {
            lock (_toLock)
            {
                if (_client == null)
                {
                    _client = new TinyHttpClient(Program.Client, _serverUrl);
                }
            }

            return _client;
        }

        protected TinyHttpClient GetClientXML()
        {
            lock (_toLock)
            {
                if (_clientXML == null)
                {
                    _clientXML = new TinyHttpClient(Program.Client, _serverUrl);

                    var xmlFormatter = _clientXML.Formatters.Where(f => f is XmlFormatter).First();
                    var jsonFormatter = _clientXML.Formatters.Where(f => f is JsonFormatter).First();
                    _clientXML.AddFormatter(new XmlFormatter(), true);
                    _clientXML.RemoveFormatter(jsonFormatter);
                    _clientXML.RemoveFormatter(xmlFormatter);
                }
            }

            return _clientXML;
        }

        protected byte[] GetByteArray(uint size)
        {
            var byteArray = new byte[size];
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = i % 2 == 0 ? (byte)0 : (byte)1;
            }

            return byteArray;
        }
    }
}
