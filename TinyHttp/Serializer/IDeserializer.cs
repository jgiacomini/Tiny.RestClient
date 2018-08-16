using System.IO;
using System.Threading.Tasks;
namespace Tiny.Http
{
    public interface IDeserializer
    {
        T Deserialize<T>(Stream stream);
    }
}