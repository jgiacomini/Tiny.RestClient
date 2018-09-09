namespace Tiny.RestClient
{
    internal abstract class BaseContent<T> : IContent
    {
        public BaseContent(T data, string contentType)
        {
            Data = data;
            ContentType = contentType;
        }

        public T Data { get; }
        public string ContentType { get; }
    }
}
