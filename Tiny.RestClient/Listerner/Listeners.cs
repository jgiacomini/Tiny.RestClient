using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
        public void OnSendingRequest(Uri uri, HttpMethod httpMethod, HttpRequestMessage httpRequestMessage)
        {
            foreach (var item in this)
            {
                try
                {
                    item.OnSendingRequest(uri, httpMethod, httpRequestMessage);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <inheritdoc/>
        public void OnFailedToReceiveResponse(Uri uri, HttpMethod httpMethod, Exception exception, TimeSpan? elapsedTime)
        {
            foreach (var item in this)
            {
                try
                {
                    item.OnFailedToReceiveResponse(uri, httpMethod, exception, elapsedTime);
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <inheritdoc/>
        public void OnReceivedResponse(Uri uri, HttpMethod httpMethod, HttpResponseMessage response, TimeSpan? elapsedTime)
        {
            foreach (var item in this)
            {
                try
                {
                    item.OnReceivedResponse(uri, httpMethod, response, elapsedTime);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
