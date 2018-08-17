using System.Text;

namespace Tiny.Http
{
    public interface ISerializer
    {
        bool HasMediaType { get; }
        string MediaType { get; }

        string Serialize<T>(T data, Encoding encoding);
    }
}