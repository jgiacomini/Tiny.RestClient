using System.IO;

namespace Tiny.Http
{
    public interface IMultiPartFromDataRequest
    {
        IMultiPartFromDataRequest AddByteArray(byte[] data, string name = null, string fileName = null, string contentType = "application/octet-stream");
        IMultiPartFromDataRequest AddStream(Stream data, string name = null, string fileName = null, string contentType = "application/octet-stream");
    }
}
