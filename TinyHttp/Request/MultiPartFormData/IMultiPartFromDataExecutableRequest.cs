using System.IO;

namespace Tiny.Http
{
    public interface IMultiPartFromDataExecutableRequest : IExecutableRequest, IWithNoStandardResponse
    {
        IMultiPartFromDataExecutableRequest AddByteArray(byte[] data, string name = null, string fileName = null, string contentType = "application/octet-stream");
        IMultiPartFromDataExecutableRequest AddStream(Stream data, string name = null, string fileName = null, string contentType = "application/octet-stream");
    }
}
