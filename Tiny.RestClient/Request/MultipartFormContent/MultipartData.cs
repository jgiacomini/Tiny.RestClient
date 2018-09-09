namespace Tiny.RestClient
{
    internal abstract class MultipartData : IContent
    {
        public MultipartData(string name, string fileName, string contentType)
        {
            Name = name;
            FileName = fileName;
            ContentType = contentType;
        }

        public string Name { get; protected set; }
        public string FileName { get; protected set; }
        public string ContentType { get; protected set; }
    }
}