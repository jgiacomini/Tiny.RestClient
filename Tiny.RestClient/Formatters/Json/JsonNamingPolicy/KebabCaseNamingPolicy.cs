namespace Tiny.RestClient
{
    internal sealed class KebabCaseNamingPolicy : JsonSeparatorNamingPolicy
    {
        public KebabCaseNamingPolicy()
            : base(lowercase: true, separator: '-')
        {
        }
    }
}