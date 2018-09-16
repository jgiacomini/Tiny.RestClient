using System.Linq;
namespace Tiny.RestClient.Tests
{
    public class BaseTest
    {
        private static readonly object _toLock = new object();
        private static TinyRestClient _client;
        private static TinyRestClient _clientXML;
        protected static readonly string _serverUrl = "http://localhost:4242/api/";

        protected TinyRestClient GetClientForUrl(string url)
        {
            return new TinyRestClient(Program.Client, url);
        }

        public static TinyRestClient GetClient()
        {
            lock (_toLock)
            {
                if (_client == null)
                {
                    _client = new TinyRestClient(Program.Client, _serverUrl);
                    _client.Settings.Listeners.AddPostMan("tests");
                }
            }

            return _client;
        }

        public static TinyRestClient GetClientXML()
        {
            lock (_toLock)
            {
                if (_clientXML == null)
                {
                    _clientXML = new TinyRestClient(Program.Client, _serverUrl);

                    var xmlFormatter = _clientXML.Settings.Formatters.Where(f => f is XmlFormatter).First();
                    var jsonFormatter = _clientXML.Settings.Formatters.Where(f => f is JsonFormatter).First();
                    _clientXML.Settings.Formatters.Add(new XmlFormatter(), true);
                    _clientXML.Settings.Formatters.Remove(jsonFormatter);
                    _clientXML.Settings.Formatters.Remove(xmlFormatter);
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
