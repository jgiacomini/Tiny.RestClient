using System.IO;

namespace Tiny.Http
{
    public interface IMultiPartFromDataRequest
    {
        IMultiPartFromDataExecutableRequest AddByteArray(byte[] data, string name = null, string fileName = null, string contentType = "application/octet-stream");
        IMultiPartFromDataExecutableRequest AddStream(Stream data, string name = null, string fileName = null, string contentType = "application/octet-stream");
        IMultiPartFromDataExecutableRequest AddContent<TContent>(TContent content, string name = null, string fileName = null, ISerializer serializer = null);
    }
}
