using System.Text;

namespace Tiny.Http
{
    internal interface IToSerializeContent
    {
        string GetSerializedStream(ISerializer serializer, Encoding encoding);
    }
}