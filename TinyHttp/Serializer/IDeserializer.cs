using System.Threading.Tasks;
namespace TinyHttp
{
    public interface IDeserializer
    {
        T Deserialize<T>(string data);
    }
}