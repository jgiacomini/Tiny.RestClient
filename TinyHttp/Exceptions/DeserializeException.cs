using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny.Http
{
    public class DeserializeException : TinyHttpException
    {
        public DeserializeException(string message, Exception innerException, string dataToDeserialize)
            : base(message, innerException)
        {
            DataToDeserialize = dataToDeserialize;
        }

        public string DataToDeserialize
        {
            get => (string)Data[nameof(DataToDeserialize)];
            private set => Data[nameof(DataToDeserialize)] = value;
        }
    }
}
