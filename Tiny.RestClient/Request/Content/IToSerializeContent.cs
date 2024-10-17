using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    internal interface IToSerializeContent
    {
        Type TypeToSerialize { get; }
        IFormatter Serializer { get; }
        Task<string> GetSerializedStringAsync(IFormatter serializer, Encoding encoding, CancellationToken cancellationToken);
    }
}