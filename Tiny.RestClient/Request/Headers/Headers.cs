using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tiny.RestClient
{
    /// <inheritdoc/>
    public class Headers : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        private IEnumerable<KeyValuePair<string, IEnumerable<string>>> _source;

        internal Headers()
        {
        }

        internal void AddSource(IEnumerable<KeyValuePair<string, IEnumerable<string>>> source)
        {
            _source = source;
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _source.GetEnumerator();
        }
    }
}
