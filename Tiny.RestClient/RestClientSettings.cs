using System;
using System.Text;

namespace Tiny.RestClient
{
    /// <summary>
    /// All settings of <see cref="TinyRestClient"/>.
    /// </summary>
    public class RestClientSettings
    {
        private Encoding _encoding;

        internal RestClientSettings()
        {
            DefaultHeaders = new Headers();
            Listeners = new Listeners();
            Formatters = new Formatters();
            Compressions = new Compressions
            {
                new GzipCompression(),
                new DeflateCompression()
            };
            _encoding = Encoding.UTF8;
            DefaultTimeout = TimeSpan.FromSeconds(100);
        }

        /// <summary>
        /// Add to all request the AcceptLanguage based on CurrentCulture of the Thread.
        /// </summary>
        public bool AddAcceptLanguageBasedOnCurrentCulture { get; set; }

        /// <summary>
        /// Get or set the ETagContainer.
        /// </summary>
        public IETagContainer ETagContainer { get; set; }

        /// <summary>
        /// Get or set the default timeout of each request.
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// Gets or set the encoding use by the client.
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value ?? throw new ArgumentNullException(nameof(Encoding));
            }
        }

        /// <summary>
        /// Gets the default headers.
        /// </summary>
        /// <value>
        /// The default headers.
        /// </value>
        public Headers DefaultHeaders
        {
            get; private set;
        }

        /// <summary>
        /// Log all requests.
        /// </summary>
        public Listeners Listeners { get; private set; }

        /// <summary>
        /// Gets the list of formatter used to serialize and deserialize data.
        /// </summary>
        public Formatters Formatters { get; private set; }

        /// <summary>
        /// Compression / decompression system enabled.
        /// </summary>
        public Compressions Compressions { get; private set; }

        /// <summary>
        /// Gets or set the handler used when HttpException will be throw (can be used to transform exception).
        /// </summary>
        public Func<HttpException, Exception> EncapsulateHttpExceptionHandler { get; set; }
    }
}