namespace Tiny.Http
{
    internal class BytesContent : BaseContent<byte[]>
    {
        public BytesContent(byte[] data, string contentType)
            : base(data, contentType)
        {
        }
    }
}