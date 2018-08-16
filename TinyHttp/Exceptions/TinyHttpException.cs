using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny.Http
{
    public abstract class TinyHttpException : Exception
    {
        public TinyHttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
