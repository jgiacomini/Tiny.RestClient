namespace TinyHttp
{
    public interface ISerializer
    {
        bool HasMediaType { get; }
        string MediaType { get; }

        string Serialize<T>(T data);
    }
}