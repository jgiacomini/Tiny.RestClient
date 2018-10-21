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
        public IActionResult GetPdf()
        {
            return File("pdf-sample.pdf", "application/pdf", "pdf-sample");
        }

        [HttpGet("NoResult")]
        public void NoResult()
        {
        }
    }
}