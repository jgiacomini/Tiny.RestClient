using System;
using System.Text;

namespace Tiny.RestClient
{
    internal interface IToSerializeContent
    {
        Type TypeToSerialize { get; }
        IFormatter Serializer { get; }
        ICompression Compression { get; }
        string GetSerializedString(IFormatter serializer, Encoding encoding);
    }
}