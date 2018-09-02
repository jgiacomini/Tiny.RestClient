using System.IO;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IMultiPartFromDataRequest
    /// </summary>
    /// <seealso cref="IMultipartFromDataRequest"/>
    /// <seealso cref="IExecutableRequest"/>
    public interface IMultiPartFromDataExecutableRequest : IMultipartFromDataRequest, IExecutableRequest
    {
    }
}
