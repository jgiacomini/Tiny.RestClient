using Microsoft.AspNetCore.Http;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.RestClient.ForTest.Api.Middleware
{
    public class CompressionMiddleware
    {
        private readonly RequestDelegate _next;

        public CompressionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var content = context.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrWhiteSpace(content))
            {
                if (content.Contains("br"))
                {
                    context.Request.Body = new BrotliStream(context.Request.Body, CompressionMode.Decompress);
                }
                else if (content.Contains("deflate"))
                {
                    context.Request.Body = new DeflateStream(context.Request.Body, CompressionMode.Decompress);
                }
            }

            await _next(context);
        }
    }
}
