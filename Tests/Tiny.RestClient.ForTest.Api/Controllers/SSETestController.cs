using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    [Route("api/SSETest")]
    [ApiController]
    public class SSETestController : ControllerBase
    {
        /// <summary>
        /// Emits 3 simple events (data only).
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("Simple")]
        public async Task Simple()
        {
            PrepareSSE();
            for (int i = 0; i < 3; i++)
            {
                await WriteAsync($"data: message {i}\n\n");
            }
        }

        /// <summary>
        /// Emits events using all standard SSE fields (id, event, data, retry).
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("AllFields")]
        public async Task AllFields()
        {
            PrepareSSE();
            await WriteAsync("id: 1\nevent: greeting\nretry: 5000\ndata: hello\n\n");
            await WriteAsync("id: 2\nevent: farewell\ndata: bye\n\n");
        }

        /// <summary>
        /// Emits a single event whose data spans multiple 'data:' lines.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("MultilineData")]
        public async Task MultilineData()
        {
            PrepareSSE();
            await WriteAsync("data: line1\ndata: line2\ndata: line3\n\n");
        }

        /// <summary>
        /// Emits events surrounded by comment lines (starting with ':') and unknown fields
        /// that must be ignored by the parser.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("WithCommentsAndUnknownFields")]
        public async Task WithCommentsAndUnknownFields()
        {
            PrepareSSE();

            // Comment + unknown field + real data.
            await WriteAsync(": this is a comment\nunknown: ignored\ndata: real data\n\n");

            // Comment-only block : must NOT produce an event.
            await WriteAsync(": keep-alive\n\n");

            // A second real event.
            await WriteAsync("data: second\n\n");
        }

        /// <summary>
        /// Emits an event with a value that has no leading space after the colon.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("NoLeadingSpace")]
        public async Task NoLeadingSpace()
        {
            PrepareSSE();
            await WriteAsync("data:nospace\n\n");
        }

        /// <summary>
        /// Emits events indefinitely (until the client disconnects) to test cancellation.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("Infinite")]
        public async Task Infinite()
        {
            PrepareSSE();
            int i = 0;
            while (!HttpContext.RequestAborted.IsCancellationRequested)
            {
                await WriteAsync($"data: tick {i}\n\n");
                i++;
                try
                {
                    await Task.Delay(20, HttpContext.RequestAborted);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        private void PrepareSSE()
        {
            Response.ContentType = "text/event-stream";

            // Ensure the response is streamed and not buffered.
            var bodyFeature = HttpContext.Features.Get<IHttpResponseBodyFeature>();
            bodyFeature?.DisableBuffering();
        }

        private async Task WriteAsync(string payload)
        {
            var bytes = Encoding.UTF8.GetBytes(payload);
            var token = HttpContext.RequestAborted;
            await Response.Body.WriteAsync(bytes, token);
            await Response.Body.FlushAsync(token);
        }
    }
}
