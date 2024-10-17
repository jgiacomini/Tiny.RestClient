using System.Text.Json;

namespace Tiny.RestClient
{
    internal static class JsonNamingPolicies
    {
        private static JsonNamingPolicy _snakeCase;
        private static JsonNamingPolicy _kebabCase;

        public static JsonNamingPolicy SnakeCase
        {
            get
            {
                return _snakeCase ??= new SnakeCaseNamingPolicy();
            }
        }

        public static JsonNamingPolicy KebabCase
        {
            get
            {
                return _kebabCase ??= new KebabCaseNamingPolicy();
            }
        }
    }
}