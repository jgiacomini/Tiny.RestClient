using System.Text;

namespace Tiny.Http
{
    internal interface IToSerializeContent
    {
        IFormatter Serializer { get; }
        string GetSerializedStream(IFormatter serializer, Encoding encoding);
    }
}