using System.IO;

namespace Tiny.Http
{
    public interface IRequest : IContentRequest, IFormRequest
    {
        IContentRequest AddContent<TContent>(TContent content);
        IContentRequest AddOctectStreamContent(byte[] byteArray);
        IContentRequest AddStreamContent(Stream stream);
    }
}