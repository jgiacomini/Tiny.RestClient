namespace Tiny.Http
{
    internal abstract class BaseContent<T> : ITinyContent
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
