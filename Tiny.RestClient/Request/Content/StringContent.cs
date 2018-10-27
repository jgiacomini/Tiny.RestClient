namespace Tiny.RestClient
{
    internal class StringContent : BaseContent<string>
    {
        public StringContent(string data, string contentType)
            : base(data, contentType)
        {
        }
    }
}