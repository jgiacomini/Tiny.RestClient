namespace Tiny.Http
{
    public interface IContentRequest : ICommonResquest, IExecutableRequest
    {
        IOctectStreamRequest WithByteArrayResponse();
        IStreamRequest WithStreamResponse();
        IRequest SerializeWith(ISerializer serializer);
        IRequest DeserializeWith(IDeserializer deserializer);
    }
}