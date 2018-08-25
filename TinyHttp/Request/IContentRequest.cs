namespace Tiny.Http
{
    /// <summary>
    /// Interface IContentRequest
    /// </summary>
    /// <seealso cref="Tiny.Http.ICommonResquest" />
    /// <seealso cref="Tiny.Http.IWithNoStandardResponse" />
    /// <seealso cref="Tiny.Http.IExecutableRequest" />
    public interface IContentRequest : ICommonResquest, IWithNoStandardResponse, IExecutableRequest
    {
    }
}