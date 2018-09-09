using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    [Route("api/File")]
    [ApiController]
    public class FileController : Controller
    {
        [HttpPost("One")]
        public async Task<string> One()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
               var result = await reader.ReadToEndAsync();

                return result.TrimEnd();
            }
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> Download()
        {
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot",
                           "pdf-sample.pdf");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(path));
        }

        [HttpGet("NoResult")]
        public void NoResult()
        {
        }
    }
}