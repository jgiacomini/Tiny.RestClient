using System.Collections;
using System.Collections.Generic;

namespace Tiny.RestClient
{
    /// <summary>
    /// Represent headers of requests
    /// </summary>
    public class Compressions : IEnumerable<KeyValuePair<string, ICompression>>
    {
        private Dictionary<string, ICompression> _compressions;

        internal Compressions()
        {
            _compressions = new Dictionary<string, ICompression>();
        }

        /// <summary>
        /// Add header
        /// </summary>
        /// <param name="compression">header name</param>
        public void Add(ICompression compression)
        {
            if (!_compressions.ContainsKey(compression.ContentEncoding))
            {
                _compressions.Add(compression.ContentEncoding, compression);
            }
            else
            {
                _compressions[compression.ContentEncoding] = compression;
            }
        }

        /// <summary>
        /// Removes the compression
        /// </summary>
        /// <param name="compression">item to remove</param>
        /// <returns></returns>
        public bool Remove(ICompression compression)
        {
            return _compressions.Remove(compression.ContentEncoding);
        }

        /// <summary>
        /// Determines whether the <see cref="Compressions"/> contains the specified compression system
        /// </summary>
        /// <param name="contentEncoding">content encoding</param>
        /// <returns>returns true if contains an element with this contentEncoding otherwise false</returns>
        public bool Contains(string contentEncoding)
        {
            return _compressions.ContainsKey(contentEncoding);
        }

        /// <summary>
        /// Gets or sets Compression system
        /// </summary>
        /// <param name="contentEncoding">content encoding key</param>
        /// <returns>return compression system</returns>
        public ICompression this[string contentEncoding]
        {
            get
            {
                return _compressions[contentEncoding];
            }
            set
            {
                _compressions[contentEncoding] = value;
            }
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, ICompression>> GetEnumerator()
        {
            return _compressions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _compressions.GetEnumerator();
        }

        /// <summary>
        /// Removes all <see cref="ICompression"/> system
        /// /// </summary>
        public void Clear()
        {
            _compressions.Clear();
        }
    }
}
