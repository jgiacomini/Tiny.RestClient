namespace Tiny.Http
{
    /// <summary>
    /// Interface ICommonResquest
    /// </summary>
    public interface ICommonResquest
    {
        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddHeader(string key, string value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, string value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, int value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, uint value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, double value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, decimal value);
    }
}