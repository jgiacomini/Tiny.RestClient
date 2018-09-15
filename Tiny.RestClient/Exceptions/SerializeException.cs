using System;

namespace Tiny.RestClient
{
    /// <summary>
    /// Class SerializeException.
    /// </summary>
    /// <seealso cref="TinyHttpException" />
    public class SerializeException : TinyHttpException
    {
        internal SerializeException(Type typeToSerialiser, Exception innerException)
            : base($"Unable to serialize {typeToSerialiser.Name}", innerException)
        {
        }
    }
}
