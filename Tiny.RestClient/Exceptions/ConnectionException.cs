﻿using System;

namespace Tiny.RestClient
{
    /// <summary>
    /// Class ConnectionException.
    /// </summary>
    /// <seealso cref="TinyRestClientException" />
    public class ConnectionException : TinyRestClientException
    {
        internal ConnectionException(string message, string url, string verb, Exception innerException)
            : base($"{message} url : {url}, Verb {verb}", innerException)
        {
            Url = url;
            Verb = verb;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url
        {
            get => (string)Data[nameof(Url)];
            private set => Data[nameof(Url)] = value;
        }

        /// <summary>
        /// Gets the verb.
        /// </summary>
        /// <value>The verb.</value>
        public string Verb
        {
            get => (string)Data[nameof(Verb)];
            private set => Data[nameof(Verb)] = value;
        }
    }
}
