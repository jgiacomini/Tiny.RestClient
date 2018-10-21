using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Tiny.RestClient.ForTest.Api.Middleware
{
    public class ETagMiddleware
    {
        private readonly RequestDelegate _next;

        public ETagMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var response = context.Response;
            using (var ms = new MemoryStream())
            {
                if (IsETagSupported(response))
                {
                    var originalStream = response.Body;
                    string checksum = CalculateChecksum(ms);

                    response.Headers[HeaderNames.ETag] = checksum;

                    if (context.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etag) && checksum == etag)
                    {
                        response.StatusCode = StatusCodes.Status304NotModified;
                        response.Body = ms;
                        await _next(context);
                        return;
                    }

                    await _next(context);
                }
                else
                {
                    await _next(context);
                }
            }
        }

        private static bool IsETagSupported(HttpResponse response)
        {
            if (response.StatusCode != StatusCodes.Status200OK)
            {
                return false;
            }

            if (response.Headers.ContainsKey(HeaderNames.ETag))
            {
                return false;
            }

            return true;
        }

        private static string CalculateChecksum(MemoryStream ms)
        {
            string checksum = string.Empty;

            using (var algo = SHA1.Create())
            {
                ms.Position = 0;
                byte[] bytes = algo.ComputeHash(ms);
                checksum = $"\"{WebEncoders.Base64UrlEncode(bytes)}\"";
            }

            return checksum;
        }
    }
}
