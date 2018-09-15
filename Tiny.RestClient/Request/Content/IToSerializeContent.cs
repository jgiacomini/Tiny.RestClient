using System.Text;

namespace Tiny.RestClient
{
    internal interface IToSerializeContent
    {
        IFormatter Serializer { get; }
        string GetSerializedString(IFormatter serializer, Encoding encoding);
    }
}