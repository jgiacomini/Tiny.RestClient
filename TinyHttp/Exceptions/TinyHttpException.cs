using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny.RestClient
{
    /// <summary>
    /// Base Class of all <see cref="TinyHttpException"/>.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public abstract class TinyHttpException : Exception
    {
        internal TinyHttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
