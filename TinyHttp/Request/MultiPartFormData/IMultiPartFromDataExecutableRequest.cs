using System.IO;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IMultiPartFromDataRequest
    /// </summary>
    /// <seealso cref="IMultiPartFromDataRequest"/>
    /// <seealso cref="IExecutableRequest"/>
    /// <seealso cref="IWithNoStandardResponse"/>
    public interface IMultiPartFromDataExecutableRequest : IMultiPartFromDataRequest, IExecutableRequest, IWithNoStandardResponse
    {
    }
}
