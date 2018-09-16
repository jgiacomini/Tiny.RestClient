using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny.RestClient
{
    /// <summary>
    /// Base Class of all <see cref="TinyRestClientException"/>.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public abstract class TinyRestClientException : Exception
    {
        internal TinyRestClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
