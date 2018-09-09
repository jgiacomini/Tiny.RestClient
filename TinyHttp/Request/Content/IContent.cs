using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny.RestClient
{
    internal interface IContent
    {
        string ContentType { get; }
    }
}
