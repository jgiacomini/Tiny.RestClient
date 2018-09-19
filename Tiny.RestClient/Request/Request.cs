using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Class TinyRequest.
    /// </summary>
    /// <seealso cref="IRequest" />
    internal class Request :
        IRequest,
        IMultipartFromDataRequest,
        IMultiPartFromDataExecutableRequest
    {
        private static readonly NumberFormatInfo _nfi;
        private readonly HttpMethod _httpMethod;
        private readonly TinyRestClient _client;
        private readonly string _route;
        private Headers _headers;
        private Dictionary<string, string> _queryParameters;
        private IContent _content;
        private List<KeyValuePair<string, string>> _formParameters;
        private MultipartContent _multiPartFormData;
        private Headers _reponseHeaders;
        private TimeSpan? _timeout;

        internal HttpMethod HttpMethod { get => _httpMethod; }
        internal Dictionary<string, string> QueryParameters { get => _queryParameters; }
        internal string Route { get => _route; }
        internal IContent Content { get => _content; }
        internal Headers ReponseHeaders { get => _reponseHeaders; }
        internal Headers Headers { get => _headers; }
        internal TimeSpan? Timeout { get => _timeout; }

        static Request()
        {
            _nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };
        }

        internal Request(HttpMethod httpMethod, string route, TinyRestClient client)
        {
            _httpMethod = httpMethod;
            _route = route;
            _client = client;
        }

        #region Content
        public IParameterRequest AddContent<TContent>(TContent content, IFormatter serializer)
        {
            _content = new ToSerializeContent<TContent>(content, serializer);
            return this;
        }

        public IParameterRequest AddByteArrayContent(byte[] byteArray, string contentType)
        {
            _content = new BytesContent(byteArray, contentType);
            return this;
        }

        public IParameterRequest AddStreamContent(Stream stream, string contentType)
        {
            _content = new StreamContent(stream, contentType);
            return this;
        }

        public IParameterRequest AddFileContent(FileInfo content, string contentType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!content.Exists)
            {
                throw new FileNotFoundException("File not found", content.FullName);
            }

            _content = new FileContent(content, contentType);
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
        public IParameterRequest FillResponseHeaders(out Headers headers)
        {
            headers = new Headers();
            _reponseHeaders = headers;
            return this;
        }

        /// <inheritdoc/>
        public IParameterRequest AddHeader(string key, string value)
        {
            if (_headers == null)
            {
                _headers = new Headers();
            }

            _headers.Add(key, value);
            return this;
        }

        /// <inheritdoc/>
        public IRequest WithBasicAuthentication(string username, string password)
        {
            if (_headers == null)
            {
                _headers = new Headers();
            }

            _headers.AddBasicAuthentication(username, password);
            return this;
        }

        /// <inheritdoc/>
        public IRequest WithOAuthBearer(string token)
        {
            if (_headers == null)
            {
                _headers = new Headers();
            }

            _headers.AddBearer(token);
            return this;
        }
        #endregion

        #region Query Parameters

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, string value)
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
        public IParameterRequest AddQueryParameter(string key, int value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, int? value)
        {
            if (value.HasValue)
            {
                return AddQueryParameter(key, value.Value.ToString());
            }
            else
            {
                AddQueryParameter(key, string.Empty);
            }

            return this;
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, uint value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, uint? value)
        {
            if (value.HasValue)
            {
                return AddQueryParameter(key, value.Value.ToString());
            }
            else
            {
                AddQueryParameter(key, string.Empty);
            }

            return this;
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, double value)
        {
            return AddQueryParameter(key, value.ToString(_nfi));
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, double? value)
        {
            if (value.HasValue)
            {
                return AddQueryParameter(key, value.Value.ToString(_nfi));
            }
            else
            {
                AddQueryParameter(key, string.Empty);
            }

            return this;
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, decimal value)
        {
            return AddQueryParameter(key, value.ToString(_nfi));
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, decimal? value)
        {
            if (value.HasValue)
            {
                return AddQueryParameter(key, value.Value.ToString(_nfi));
            }
            else
            {
                AddQueryParameter(key, string.Empty);
            }

            return this;
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, bool value)
        {
            return AddQueryParameter(key, value.ToString());
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, bool? value)
        {
            if (value.HasValue)
            {
                return AddQueryParameter(key, value.Value.ToString());
            }
            else
            {
                AddQueryParameter(key, string.Empty);
            }

            return this;
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, float value)
        {
            return AddQueryParameter(key, value.ToString(_nfi));
        }

        /// <inheritdoc/>
        public IParameterRequest AddQueryParameter(string key, float? value)
        {
            if (value.HasValue)
            {
                return AddQueryParameter(key, value.Value.ToString(_nfi));
            }
            else
            {
                AddQueryParameter(key, string.Empty);
            }

            return this;
        }
        #endregion

        /// <inheritdoc/>
        public IRequest WithTimeout(TimeSpan timeout)
        {
            _timeout = timeout;

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

        /// <inheritdoc/>
        public Task<byte[]> ExecuteAsByteArrayAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsByteArrayResultAsync(this, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<Stream> ExecuteAsStreamAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsStreamResultAsync(this, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<string> ExecuteAsStringAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsStringResultAsync(this, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> ExecuteAsHttpResponseMessageAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsHttpResponseMessageResultAsync(this, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<FileInfo> DownloadFileAsync(string fileName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(File));
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var fileStream = File.Create(fileName))
            {
                using (var stream = await _client.ExecuteAsStreamResultAsync(this, cancellationToken))
                {
                    if (stream != null)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }
                }
            }

            return new FileInfo(fileName);
        }

        #region MultiPart

        /// <inheritdoc/>
        public IMultipartFromDataRequest AsMultiPartFromDataRequest(string contentType)
        {
            _multiPartFormData = new MultipartContent(contentType);
            _content = _multiPartFormData;
            return this;
        }

        /// <inheritdoc/>
        IMultiPartFromDataExecutableRequest IMultipartFromDataRequest.AddByteArray(byte[] data, string name, string fileName, string contentType)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _multiPartFormData.Add(new BytesMultipartData(data, name, fileName, contentType));

            return this;
        }

        /// <inheritdoc/>
        IMultiPartFromDataExecutableRequest IMultipartFromDataRequest.AddStream(Stream data, string name, string fileName, string contentType)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _multiPartFormData.Add(new StreamMultipartData(data, name, fileName, contentType));

            return this;
        }

        /// <inheritdoc/>
        IMultiPartFromDataExecutableRequest IMultipartFromDataRequest.AddContent<TContent>(TContent content, string name, string fileName, IFormatter serializer)
        {
            if (content == default)
            {
                throw new ArgumentNullException(nameof(content));
            }

            _multiPartFormData.Add(new ToSerializeMultipartData<TContent>(content, name, fileName, serializer));

            return this;
        }

        IMultiPartFromDataExecutableRequest IMultipartFromDataRequest.AddFileContent(FileInfo content, string contentType)
        {
            IMultipartFromDataRequest me = this;
            return me.AddFileContent(content, null, null, contentType);
        }

        IMultiPartFromDataExecutableRequest IMultipartFromDataRequest.AddFileContent(FileInfo content, string name, string fileName, string contentType)
        {
            if (content == default)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!content.Exists)
            {
                throw new FileNotFoundException("File not found", content.FullName);
            }

            if (name == null)
            {
                name = content.Name;
            }

            if (fileName == null)
            {
                fileName = $"{content.Name}.{content.Extension}";
            }

            _multiPartFormData.Add(new FileMultipartData(content, name, fileName, contentType));

            return this;
        }
        #endregion
    }
}