using System.Collections;
using System.Collections.Generic;

namespace Tiny.RestClient
{
    /// <inheritdoc/>
    public class Headers : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        private List<KeyValuePair<string, IEnumerable<string>>> _headers;

        internal Headers()
        {
            _headers = new List<KeyValuePair<string, IEnumerable<string>>>();
        }

        internal void AddRange(IEnumerable<KeyValuePair<string, IEnumerable<string>>> range)
        {
            _headers.AddRange(range);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _headers.GetEnumerator();
        }
    }
}
