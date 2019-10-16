namespace Tiny.RestClient
{
    /// <summary>
    /// Interface IParameterRequest.
    /// </summary>
    /// <seealso cref="IExecutableRequest" />
    public interface IParameterRequest : IExecutableRequest
    {
        /// <summary>
        /// Fill header of response.
        /// </summary>
        /// <param name="headers">Header filled after execute method.</param>
        /// <returns>The current request.</returns>
        IParameterRequest FillResponseHeaders(out Headers headers);

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddHeader(string key, string value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, string value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, bool value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, bool? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, int value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, int? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, uint value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, uint? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, double value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, double? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, decimal value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, decimal? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, float value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, float? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, long? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddQueryParameter(string key, long value);
    }
}
