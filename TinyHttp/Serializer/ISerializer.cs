namespace TinyHttp
{
    public interface ISerializer
    {
        string MediaType { get; }

        string Serialize<T>(T data);
    }
}