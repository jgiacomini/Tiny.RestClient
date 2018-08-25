namespace Tiny.Http
{
    internal class BytesMultiPartData : MultiPartData
    {
        public BytesMultiPartData(byte[] data, string name, string fileName, string contentType)
        {
            Data = data;
            Name = name;
            FileName = fileName;
            ContentType = contentType;
        }

        public byte[] Data { get; }
    }
}