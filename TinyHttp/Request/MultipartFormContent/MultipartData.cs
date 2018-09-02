namespace Tiny.Http
{
    internal abstract class MultipartData : ITinyContent
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