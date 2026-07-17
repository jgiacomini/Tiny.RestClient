using System;
using System.Collections;
using System.Collections.Generic;

namespace Tiny.RestClient
{
    /// <summary>
    /// Collection of <see cref="IFormatter"/>.
    /// </summary>
    public sealed class Formatters : IEnumerable<IFormatter>
    {
        private readonly List<IFormatter> _formatters;
        private IFormatter _defaultFormatter;

        internal Formatters()
        {
            _defaultFormatter = new JsonFormatter();
            _formatters = new List<IFormatter>
            {
                _defaultFormatter,
                new XmlFormatter()
            };
        }

        /// <summary>
        /// Gets the default <see cref="IFormatter"/>.
        /// </summary>
        public IFormatter Default
        {
            get
            {
                return _defaultFormatter;
            }
        }

        /// <summary>
        /// Add a formatter in the list of supported formatters.
        /// </summary>
        /// <param name="formatter">Add the formatter to the list of supported formatter. The value can't be null.</param>
        /// <param name="isDefault">Define this formatter as default formatter.</param>
        /// <exception cref="ArgumentNullException">throw <see cref="ArgumentNullException"/> if formatter is null.</exception>
        public void Add(IFormatter formatter, bool isDefault)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            if (isDefault)
            {
                _defaultFormatter = formatter;
            }

            _formatters.Insert(0, formatter);
        }

        /// <summary>
        /// Removes a formatter in the list of supported formatters.
        /// </summary>
        /// <param name="formatter">The formatter to remove on the supported formatter list.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found.</returns>
        /// <exception cref="ArgumentNullException">throw <see cref="ArgumentNullException"/> if formatter is null.</exception>
        /// <exception cref="ArgumentException">throw <see cref="ArgumentException"/> if the current formatter removed is the default one. </exception>
        public bool Remove(IFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            if (_defaultFormatter == formatter)
            {
                throw new ArgumentException("Add a new default formatter before remove the current one");
            }

            bool result = _formatters.Remove(formatter);

            return result;
        }

        IEnumerator<IFormatter> IEnumerable<IFormatter>.GetEnumerator()
        {
            return _formatters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _formatters.GetEnumerator();
        }
    }
}
