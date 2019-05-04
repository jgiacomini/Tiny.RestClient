using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Text;

namespace Tiny.RestClient.Json
{
    /// <summary>
    ///  Allow a custom separator to resolve of property (propertySEPARATORname become PropertyName).
    /// </summary>
    public class SeparatorPropertyNamesContractResolver : DefaultContractResolver
    {
        private readonly string _separator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatorPropertyNamesContractResolver"/>.
        /// </summary>
        /// <param name="separator">separator bewteen words.</param>
        public SeparatorPropertyNamesContractResolver(char separator)
        {
            _separator = separator.ToString();
        }

        /// <summary>
        /// Resolves a property name to a delimited name.
        /// </summary>
        /// <param name="propertyName">property name to resolve.</param>
        /// <returns></returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            var parts = new List<string>();
            var currentWord = new StringBuilder();

            foreach (var c in propertyName.ToCharArray())
            {
                if (char.IsUpper(c) && currentWord.Length > 0)
                {
                    parts.Add(currentWord.ToString());
                    currentWord.Clear();
                }

                currentWord.Append(char.ToLower(c));
            }

            if (currentWord.Length > 0)
            {
                parts.Add(currentWord.ToString());
            }

            return string.Join(_separator, parts.ToArray());
        }
    }
}
