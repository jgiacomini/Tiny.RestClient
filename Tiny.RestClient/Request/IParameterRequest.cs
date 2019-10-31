using System;

namespace Tiny.RestClient
{
    /// <summary>
    /// Interface IParameterRequest.
    /// </summary>
    /// <seealso cref="IExecutableRequest" />
    public interface IParameterRequest : IExecutableRequest
    {
        /// <summary>
        /// Add a basic authentication credentials.
        /// </summary>
        /// <param name="username">the username.</param>
        /// <param name="password">the password.</param>
        /// <returns>The current request.</returns>
        IParameterRequest WithBasicAuthentication(string username, string password);

        /// <summary>
        /// Add a bearer token in the request headers.
        /// </summary>
        /// <param name="token">token value.</param>
        /// <returns>The current request.</returns>
        IParameterRequest WithOAuthBearer(string token);

        /// <summary>
        /// With timeout for current request.
        /// </summary>
        /// <param name="timeout">timeout.</param>
        /// <returns>The current request.</returns>
        IParameterRequest WithTimeout(TimeSpan timeout);

        /// <summary>
        /// With a specific etag container.
        /// </summary>
        /// <param name="eTagContainer">the eTag container.</param>
        /// <returns></returns>
        IParameterRequest WithETagContainer(IETagContainer eTagContainer);

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
