using System.Text;

namespace Tiny.RestClient
{
    internal interface IToSerializeContent
    {
        IFormatter Serializer { get; }
        string GetSerializedStream(IFormatter serializer, Encoding encoding);
    }
}