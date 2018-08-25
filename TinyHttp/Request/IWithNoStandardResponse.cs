namespace Tiny.Http
{
    /// <summary>
    /// Interface IContentRequest
    /// </summary>
    /// <seealso cref="Tiny.Http.ICommonResquest" />
    /// <seealso cref="Tiny.Http.IExecutableRequest" />
    public interface IWithNoStandardResponse
    {
        /// <summary>
        /// Withes the byte array response.
        /// </summary>
        /// <returns>IOctectStreamRequest.</returns>
        IOctectStreamRequest WithByteArrayResponse();

        /// <summary>
        /// Withes the stream response.
        /// </summary>
        /// <returns>The current request</returns>
        IStreamRequest WithStreamResponse();
    }
}