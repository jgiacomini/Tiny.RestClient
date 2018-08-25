using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny.Http
{
    internal interface ITinyContent
    {
        string ContentType { get; }
    }
}
