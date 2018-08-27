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
    internal class TinyRequest : IRequest, IOctectStreamRequest, IStreamRequest, IMultiPartFromDataRequest, IMultiPartFromDataExecutableRequest
    {
        private readonly HttpVerb _httpVerb;
        private readonly TinyHttpClient _client;
        private readonly string _route;
        private Dictionary<string, string> _headers;
        private Dictionary<string, string> _queryParameters;
        private ITinyContent _content;
        private List<KeyValuePair<string, string>> _formParameters;
        private MultiPartContent _multiPartFormData;

        internal HttpVerb HttpVerb { get => _httpVerb; }
        internal Dictionary<string, string> QueryParameters { get => _queryParameters; }
        internal string Route { get => _route; }
        internal ITinyContent Content { get => _content; }

        internal TinyRequest(HttpVerb httpVerb, string route, TinyHttpClient client)
        {
            _httpVerb = httpVerb;
            _route = route;
            _client = client;
            _headers = new Dictionary<string, string>();
        }

        #region Content
        public IContentRequest AddContent<TContent>(TContent content, IFormatter serializer)
        {
            _content = new ToSerializeContent<TContent>(content, serializer);
            return this;
        }

        public IContentRequest AddByteArrayContent(byte[] byteArray, string contentType)
        {
            _content = new BytesContent(byteArray, contentType);
            return this;
        }

        public IContentRequest AddStreamContent(Stream stream, string contentType)
        {
            _content = new StreamContent(stream, contentType);
            return this;
        }

        #endregion

        #region Forms Parameters

        /// <inheritdoc/>
        public IFormRequest AddFormParameter(string key, string value)
        {
            if (_formParameters == null)
            {
                _formParameters = new List<KeyValuePair<string, string>>();
                _content = new FormParametersContent(_formParameters, null);
            }

            _formParameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        /// <inheritdoc/>
        public IFormRequest AddFormParameters(IEnumerable<KeyValuePair<string, string>> items)
        {
            if (_formParameters == null)
            {
                _formParameters = new List<KeyValuePair<string, string>>();
                _content = new FormParametersContent(_formParameters, null);
            }

            _formParameters.AddRange(items);
            return this;
        }
        #endregion

        #region Headers

        /// <inheritdoc/>
        public IRequest AddHeader(string key, string value)
        {
            if (_headers == null)
            {
                _headers = new Dictionary<string, string>();
            }

            _headers.Add(key, value);
            return this;
        }
        #endregion

        #region Query Parameters

        /// <inheritdoc/>
        public IRequest AddQueryParameter(string key, string value)
        {
            if (_queryParameters == null)
            {
                _queryParameters = new Dictionary<string, string>();
            }

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

        /// <inheritdoc/>
        public IRequest AddQueryParameter(string key, int value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <inheritdoc/>
        public IRequest AddQueryParameter(string key, uint value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <inheritdoc/>
        public IRequest AddQueryParameter(string key, double value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <inheritdoc/>
        public IRequest AddQueryParameter(string key, decimal value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <inheritdoc/>
        public IRequest AddQueryParameter(string key, bool value)
        {
            return AddQueryParameter(key, value.ToString());
        }
        #endregion

        /// <inheritdoc/>
        public IOctectStreamRequest WithByteArrayResponse()
        {
            return this;
        }

        /// <inheritdoc/>
        public IStreamRequest WithStreamResponse()
        {
            return this;
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync<TResult>(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsync<TResult>(this, null, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync<TResult>(IFormatter formatter, CancellationToken cancellationToken)
        {
            return _client.ExecuteAsync<TResult>(this, formatter, cancellationToken);
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsync(this, cancellationToken);
        }

        Task<byte[]> IOctectStreamRequest.ExecuteAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteByteArrayResultAsync(this, cancellationToken);
        }

        Task<Stream> IStreamRequest.ExecuteAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteWithStreamResultAsync(this, cancellationToken);
        }

        #region MultiPart

        /// <inheritdoc/>
        public IMultiPartFromDataRequest AsMultiPartFromDataRequest(string contentType)
        {
            _multiPartFormData = new MultiPartContent(contentType);
            _content = _multiPartFormData;
            return this;
        }

        /// <inheritdoc/>
        IMultiPartFromDataExecutableRequest IMultiPartFromDataRequest.AddByteArray(byte[] data, string name, string fileName, string contentType)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _multiPartFormData.Add(new BytesMultiPartData(data, name, fileName, contentType));

            return this;
        }

        /// <inheritdoc/>
        IMultiPartFromDataExecutableRequest IMultiPartFromDataRequest.AddStream(Stream data, string name, string fileName, string contentType)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _multiPartFormData.Add(new StreamMultiPartData(data, name, fileName, contentType));

            return this;
        }

        /// <inheritdoc/>
        IMultiPartFromDataExecutableRequest IMultiPartFromDataRequest.AddContent<TContent>(TContent content, string name, string fileName, IFormatter serializer)
        {
            if (content == default)
            {
                throw new ArgumentNullException(nameof(content));
            }

            _multiPartFormData.Add(new ToSerializeMultiPartData<TContent>(content, name, fileName, serializer));

            return this;
        }

        #endregion
    }
}