namespace Tiny.RestClient
{
    internal class StringMultipartData : MultipartData, IContent
    {
        public StringMultipartData(string data, string name, string fileName, string contentType)
            : base(name, fileName, contentType)
        {
            Data = data;
        }

        public string Data { get; }
    }
}