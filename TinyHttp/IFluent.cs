using System.Collections.Generic;

namespace TinyHttp
{
    public interface IFluent : ISerializableFluent, ISimpleRequest
    {
        Dictionary<string, string> DefaultHeaders { get; }
        IFluent AddHeader(string key, string value);
        IFluent AddQueryParameter(string key, string value);
        IFluent AddFormParameter(string key, string value);

        IFluent FromStream();
        IFluent ToStream();

        IFluent WithByteArrayResult();
        IFluent WithStreamResult();
    }
}