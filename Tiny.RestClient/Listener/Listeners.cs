using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Collection of <see cref="IListener"/>
    /// </summary>
    public class Listeners : ICollection<IListener>, IListener
    {
        private readonly List<IListener> _loggers;

        internal Listeners() => _loggers = new List<IListener>();

        /// <inheritdoc/>
        public int Count => _loggers.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => ((IList<IListener>)_loggers).IsReadOnly;

        /// <inheritdoc/>
        public bool MeasureTime { get; private set; }

        /// <inheritdoc/>
        public void Add(IListener item)
        {
            _loggers.Add(item);
            if (!MeasureTime)
            {
                MeasureTime = item.MeasureTime;
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _loggers.Clear();
            MeasureTime = false;
        }

        /// <inheritdoc/>
        public bool Contains(IListener item)
        {
            return _loggers.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(IListener[] array, int arrayIndex)
        {
            _loggers.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<IListener> GetEnumerator()
        {
            return ((IList<IListener>)_loggers).GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Remove(IListener item)
        {
            var result = _loggers.Remove(item);
            MeasureTime = _loggers.Any(l => l.MeasureTime);

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<IListener>)_loggers).GetEnumerator();
        }

        /// <inheritdoc/>
        public async Task OnSendingRequestAsync(Uri uri, HttpMethod httpMethod, HttpRequestMessage httpRequestMessage)
        {
            foreach (var item in this)
            {
                try
                {
                    await item.OnSendingRequestAsync(uri, httpMethod, httpRequestMessage).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <inheritdoc/>
        public async Task OnFailedToReceiveResponseAsync(Uri uri, HttpMethod httpMethod, Exception exception, TimeSpan? elapsedTime)
        {
            foreach (var item in this)
            {
                try
                {
                    await item.OnFailedToReceiveResponseAsync(uri, httpMethod, exception, item.MeasureTime ? elapsedTime : null).ConfigureAwait(false);
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <inheritdoc/>
        public async Task OnReceivedResponseAsync(Uri uri, HttpMethod httpMethod, HttpResponseMessage response, TimeSpan? elapsedTime)
        {
            foreach (var item in this)
            {
                try
                {
                    await item.OnReceivedResponseAsync(uri, httpMethod, response, item.MeasureTime ? elapsedTime : null).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
