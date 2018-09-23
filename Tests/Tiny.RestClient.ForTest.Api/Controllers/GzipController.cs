using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Tiny.RestClient.Tests.Models;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    [Route("api/Gzip")]
    [ApiController]
    public class GzipController : ControllerBase
    {
        [HttpPost("Complex")]
        public async Task<Response> Complex()
        {
            var body = Request.Body;

            var decompressed = await DecompressAsync(body);
            var bytes = decompressed.ToArray();
            var postRequest = JsonConvert.DeserializeObject<Request>(Encoding.UTF8.GetString(bytes));
            return new Response() { Id = postRequest.Id, ResponseData = postRequest.Data };
        }

        public async Task<MemoryStream> DecompressAsync(Stream stream)
        {
            var decompressedStream = new MemoryStream();
            using (var decompressionStream = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                await decompressionStream.CopyToAsync(decompressedStream).ConfigureAwait(false);
                await decompressionStream.FlushAsync();
            }

            return decompressedStream;
        }
    }
}
