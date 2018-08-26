using System.Text;

namespace Tiny.Http
{
    internal interface IToSerializeContent
    {
        ISerializer Serializer { get; }
        string GetSerializedStream(ISerializer serializer, Encoding encoding);
    }
}