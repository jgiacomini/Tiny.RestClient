using System.IO;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IMultiPartFromDataRequest
    /// </summary>
    /// <seealso cref="IMultiPartFromDataRequest"/>
    /// <seealso cref="IExecutableRequest"/>
    public interface IMultiPartFromDataExecutableRequest : IMultiPartFromDataRequest, IExecutableRequest
    {
    }
}
