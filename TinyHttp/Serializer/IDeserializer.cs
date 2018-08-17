using System.IO;
using System.Threading.Tasks;
namespace Tiny.Http
{
    public interface IDeserializer
    {
        bool HasMediaType { get; }
        string MediaType { get; }
        T Deserialize<T>(Stream stream);
    }
}