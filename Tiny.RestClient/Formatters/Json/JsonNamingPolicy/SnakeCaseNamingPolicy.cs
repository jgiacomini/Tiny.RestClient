namespace Tiny.RestClient
{
    internal sealed class SnakeCaseNamingPolicy : JsonSeparatorNamingPolicy
    {
        public SnakeCaseNamingPolicy()
            : base(lowercase: true, separator: '_')
        {
        }
    }
}