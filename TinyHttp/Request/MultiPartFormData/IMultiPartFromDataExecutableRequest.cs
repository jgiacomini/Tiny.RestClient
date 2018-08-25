using System.IO;

namespace Tiny.Http
{
    public interface IMultiPartFromDataExecutableRequest : IMultiPartFromDataRequest, IExecutableRequest, IWithNoStandardResponse
    {
    }
}
