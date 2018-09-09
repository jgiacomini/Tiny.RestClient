using System;

namespace Tiny.RestClient
{
    /// <summary>
    /// Class DeserializeException.
    /// </summary>
    /// <seealso cref="TinyHttpException" />
    public class DeserializeException : TinyHttpException
    {
        internal DeserializeException(string message, Exception innerException, string dataToDeserialize)
            : base(message, innerException)
        {
            DataToDeserialize = dataToDeserialize;
        }

        /// <summary>
        /// Gets the data to deserialize.
        /// </summary>
        /// <value>The data to deserialize.</value>
        public string DataToDeserialize
        {
            get => (string)Data[nameof(DataToDeserialize)];
            private set => Data[nameof(DataToDeserialize)] = value;
        }
    }
}
