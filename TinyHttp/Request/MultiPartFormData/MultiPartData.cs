namespace Tiny.Http
{
    internal abstract class MultiPartData
    {
        public string Name { get; protected set; }
        public string FileName { get; protected set; }
        public string ContentType { get; protected set; }
    }
}