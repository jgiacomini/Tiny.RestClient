using System.Collections.Generic;

namespace Tiny.Http
{
    internal class FormParametersContent : BaseContent<List<KeyValuePair<string, string>>>
    {
        public FormParametersContent(List<KeyValuePair<string, string>> data, string contentType)
            : base(data, contentType)
        {
        }
    }
}