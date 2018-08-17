using System.Collections.Generic;

namespace Tiny.Http
{
    public interface IFormRequest : ICommonResquest, IExecutableRequest
    {
        IFormRequest AddFormParameter(string key, string value);
        IFormRequest AddFormParameters(IEnumerable<KeyValuePair<string, string>> datas);
    }
}