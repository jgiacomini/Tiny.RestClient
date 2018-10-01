using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Represent headers of requests
    /// </summary>
    public class Headers : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        private Dictionary<string, IEnumerable<string>> _headers;

        private Func<Task<string>> _bearerFunc;
        private Func<Task<Tuple<string, string>>> _basicAuthenticationFunc;

        internal Headers()
        {
            _headers = new Dictionary<string, IEnumerable<string>>();
        }

        /// <summary>
        /// Add OAuth 2.0 token
        /// </summary>
        /// <param name="token">token to add</param>
        public void AddBearer(string token)
        {
            Add("Authorization", $"Bearer {token}");
        }

        /// <summary>
        /// Add OAuth 2.0 token
        /// </summary>
        /// <param name="bearerFunc">a delegate</param>
        public void AddBearer(Func<Task<string>> bearerFunc)
        {
            _bearerFunc = bearerFunc;
        }

        /// <summary>
        /// Add basic authentication
        /// </summary>
        /// <param name="username">the username</param>
        /// <param name="password">the password</param>
        public void AddBasicAuthentication(string username, string password)
        {
            var encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            Add("Authorization", $"Basic {encodedCreds}");
        }

        /// <summary>
        /// Add basic authentication
        /// </summary>
        /// <param name="basicAuthenticationFunc">a delegate</param>
        public void AddBasicAuthentication(Func<Task<Tuple<string, string>>> basicAuthenticationFunc)
        {
            _basicAuthenticationFunc = basicAuthenticationFunc;
        }

        /// <summary>
        /// Add header
        /// </summary>
        /// <param name="name">header name</param>
        /// <param name="value">header value</param>
        public void Add(string name, string value)
        {
            List<string> list = null;
            if (!_headers.ContainsKey(name))
            {
                list = new List<string>();
                _headers.Add(name, list);
            }

            list.Add(value);
        }

        /// <summary>
        /// Removes the header with specified name
        /// </summary>
        /// <param name="name">name of the header</param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            return _headers.Remove(name);
        }

        internal void Add(string key, IEnumerable<string> values)
        {
            if (!_headers.ContainsKey(key))
            {
                _headers.Add(key, values.ToList());
            }
            else
            {
                var currentList = _headers[key] as List<string>;
                currentList.AddRange(values);
            }
        }

        internal void AddRange(IEnumerable<KeyValuePair<string, IEnumerable<string>>> range)
        {
            foreach (var item in range)
            {
                Add(item.Key, item.Value);
            }
        }

        internal async Task ExecuteDynamicHeaders()
        {
            if (_bearerFunc != null)
            {
                AddBearer(await _bearerFunc.Invoke());
            }

            if (_basicAuthenticationFunc != null)
            {
                var basicAuthentication = await _basicAuthenticationFunc.Invoke();
                AddBasicAuthentication(basicAuthentication.Item1, basicAuthentication.Item2);
            }
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
