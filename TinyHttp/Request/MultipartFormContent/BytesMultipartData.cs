namespace Tiny.Http
{
    internal class BytesMultipartData : MultipartData
    {
        public BytesMultipartData(byte[] data, string name, string fileName, string contentType)
            : base(name, fileName, contentType)
        {
            Data = data;
        }

        public byte[] Data { get; }
    }
}