using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    /// <summary>
    /// Class TinyRequest.
    /// </summary>
    /// <seealso cref="Tiny.Http.IRequest" />
    /// <seealso cref="Tiny.Http.IOctectStreamRequest" />
    /// <seealso cref="Tiny.Http.IStreamRequest" />
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

        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <typeparam name="TContent">The type of the t content.</typeparam>
        /// <param name="content">The content.</param>
        /// <returns>IContentRequest.</returns>
        public IContentRequest AddContent<TContent>(TContent content)
        {
            _content = content;
            _contentType = ContentType.String;
            return this;
        }

        /// <summary>
        /// Adds the content of the byte array. (it will not use the serializer)
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>IContentRequest.</returns>
        public IContentRequest AddByteArrayContent(byte[] byteArray)
        {
            _byteArray = byteArray;
            _contentType = ContentType.ByteArray;
            return this;
        }

        /// <summary>
        /// Adds the content of the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>IContentRequest.</returns>
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

        /// <summary>
        /// Adds the form parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>IFormRequest.</returns>
        public IFormRequest AddFormParameter(string key, string value)
        {
            _formParameters.Add(new KeyValuePair<string, string>(key, value));
            _contentType = ContentType.Forms;
            return this;
        }

        /// <summary>
        /// Adds the form parameters.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>IFormRequest.</returns>
        public IFormRequest AddFormParameters(IEnumerable<KeyValuePair<string, string>> items)
        {
            _formParameters.AddRange(items);
            _contentType = ContentType.Forms;
            return this;
        }

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        public IRequest AddHeader(string key, string value)
        {
            _headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        public IRequest AddQueryParameter(string key, string value)
        {
            if (!_queryParameters.ContainsKey(key))
            {
                _queryParameters.Add(key, value);
            }
            else
            {
                // TODO : Throw an exception ?
                _queryParameters[key] = value;
            }

            return this;
        }

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        public IRequest AddQueryParameter(string key, int value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        public IRequest AddQueryParameter(string key, uint value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        public IRequest AddQueryParameter(string key, double value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        public IRequest AddQueryParameter(string key, decimal value)
        {
            return AddQueryParameter(key, value.ToString());
        }
        #endregion

        #region Serializer

        /// <summary>
        /// Serializes the with.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <returns>The current request</returns>
        public IRequest SerializeWith(ISerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        /// <summary>
        /// Deserializes the with.
        /// </summary>
        /// <param name="deserializer">The deserializer.</param>
        /// <returns>The current request</returns>
        public IRequest DeserializeWith(IDeserializer deserializer)
        {
            _deserializer = deserializer;
            return this;
        }
        #endregion

        /// <summary>
        /// Withes the byte array response.
        /// </summary>
        /// <returns>IOctectStreamRequest.</returns>
        public IOctectStreamRequest WithByteArrayResponse()
        {
            return this;
        }

        /// <summary>
        /// Withes the stream response.
        /// </summary>
        /// <returns>IStreamRequest.</returns>
        public IStreamRequest WithStreamResponse()
        {
            return this;
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task of TResult</returns>
        public Task<TResult> ExecuteAsync<TResult>(CancellationToken cancellationToken = default)
        {
            return _client.ExecuteAsync<TResult>(_httpVerb, _route, _headers, _queryParameters, _formParameters, _serializer, _deserializer, _contentType, GetContent(), cancellationToken);
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task</returns>
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return _client.ExecuteAsync(_httpVerb, _route, _headers, _queryParameters, _formParameters, _serializer, _deserializer, _contentType, GetContent(), cancellationToken);
        }

        /// <summary>
        /// Executes the request
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task of byte array.</returns>
        Task<byte[]> IOctectStreamRequest.ExecuteAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteByteArrayResultAsync(_httpVerb, _route, _headers, _queryParameters, _formParameters, _serializer, _deserializer, _contentType, GetContent(), cancellationToken);
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task of <seealso cref="Stream"/></returns>
        Task<Stream> IStreamRequest.ExecuteAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteWithStreamResultAsync(_httpVerb, _route, _headers, _queryParameters, _formParameters, _serializer, _deserializer, _contentType, GetContent(), cancellationToken);
        }
    }
}