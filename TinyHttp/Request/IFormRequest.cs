using System.Collections.Generic;

namespace Tiny.RestClient
{
    /// <summary>
    /// Interface IFormRequest
    /// </summary>
    /// <seealso cref="IParameterRequest" />
    public interface IFormRequest : IParameterRequest
    {
        /// <summary>
        /// Adds the form parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IFormRequest AddFormParameter(string key, string value);

        /// <summary>
        /// Adds the form parameters.
        /// </summary>
        /// <param name="datas">The datas.</param>
        /// <returns>The current request</returns>
        IFormRequest AddFormParameters(IEnumerable<KeyValuePair<string, string>> datas);
    }
}