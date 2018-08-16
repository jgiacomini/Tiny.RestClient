using System.Threading.Tasks;
namespace Tiny.Http
{
    public interface IDeserializer
    {
        T Deserialize<T>(string data);
    }
}