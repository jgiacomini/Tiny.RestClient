using System;

namespace Tiny.RestClient
{
    /// <summary>
    /// Class SerializeException.
    /// </summary>
    /// <seealso cref="TinyRestClientException" />
    public class SerializeException : TinyRestClientException
    {
        internal SerializeException(Type typeToSerialiser, Exception innerException)
            : base($"Unable to serialize {typeToSerialiser.Name}", innerException)
        {
        }
    }
}
