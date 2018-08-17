using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    public class TinyRequest : IRequest, IOctectStreamRequest, IStreamRequest
    {
        private readonly HttpVerb _httpVerb;
        private readonly TinyHttpClient _client;
        private readonly string _route;
        private readonly Dictionary<string, string> _headers;
        private readonly Dictionary<string, string> _queryParameters;
        private readonly List<KeyValuePair<string, string>> _formParameters;
        private ISerializer _serializer;
        private IDeserializer _deserializer;
        private object _content;
        private byte[] _byteArray;
        private Stream _contentStream;
        private ContentType _contentType;

        internal TinyRequest(HttpVerb httpVerb, string route, TinyHttpClient client)
        {
            _httpVerb = httpVerb;
            _route = route;
            _client = client;
            _headers = new Dictionary<string, string>();
            _queryParameters = new Dictionary<string, string>();
            _formParameters = new List<KeyValuePair<string, string>>();
        }

        #region Content
        public IContentRequest AddContent<TContent>(TContent content)
        {
            _content = content;
            _contentType = ContentType.String;
            return this;
        }

        public IContentRequest AddOctectStreamContent(byte[] byteArray)
        {
            _byteArray = byteArray;
            _contentType = ContentType.ByteArray;
            return this;
        }

        public IContentRequest AddStreamContent(Stream stream)
        {
            _contentStream = stream;
            _contentType = ContentType.Stream;
            return this;
        }

        private object GetContent()
        {
            switch (_contentType)
            {
                case ContentType.String:
                    return _content;
                case ContentType.Forms:
                    return null;
                case ContentType.Stream:
                    return _contentStream;
                case ContentType.ByteArray:
                    return _byteArray;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region Parameters
        public IFormRequest AddFormParameter(string key, string value)
        {
            _formParameters.Add(new KeyValuePair<string, string>(key, value));
            _contentType = ContentType.Forms;
            return this;
        }

        public IFormRequest AddFormParameters(IEnumerable<KeyValuePair<string, string>> items)
        {
            _formParameters.AddRange(items);
            _contentType = ContentType.Forms;
            return this;
        }

        public IRequest AddHeader(string key, string value)
        {
            _headers.Add(key, value);
            return this;
        }

        public IRequest AddQueryParameter(string key, string value)
        {
            if (!_queryParameters.ContainsKey(key))
            {
                _queryParameters.Add(key, value);
            }
            else
            {
                // TODO : Throw an exception
                _queryParameters[key] = value;
            }

            return this;
        }

        public IRequest AddQueryParameter(string key, int value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        public IRequest AddQueryParameter(string key, uint value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        public IRequest AddQueryParameter(string key, double value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        public IRequest AddQueryParameter(string key, decimal value)
        {
            return AddQueryParameter(key, value.ToString());
        }
        #endregion

        #region Serializer
        public IRequest SerializeWith(ISerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        public IRequest DeserializeWith(IDeserializer deserializer)
        {
            _deserializer = deserializer;
            return this;
        }
        #endregion

        public IOctectStreamRequest WithByteArrayResponse()
        {
            return this;
        }

        public IStreamRequest WithStreamResponse()
        {
            return this;
        }

        public Task<TResult> ExecuteAsync<TResult>(CancellationToken cancellationToken = default)
        {
            return _client.ExecuteAsync<TResult>(_httpVerb, _route, _headers, _queryParameters, _formParameters, _serializer, _deserializer, _contentType, GetContent(), cancellationToken);
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return _client.ExecuteAsync(_httpVerb, _route, _headers, _queryParameters, _formParameters, _serializer, _deserializer, _contentType, GetContent(), cancellationToken);
        }

        Task<byte[]> IOctectStreamRequest.ExecuteAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteByteArrayResultAsync(_httpVerb, _route, _headers, _queryParameters, _formParameters, _serializer, _deserializer, _contentType, GetContent(), cancellationToken);
        }

        Task<Stream> IStreamRequest.ExecuteAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteWithStreamResultAsync(_httpVerb, _route, _headers, _queryParameters, _formParameters, _serializer, _deserializer, _contentType, GetContent(), cancellationToken);
        }
    }
}