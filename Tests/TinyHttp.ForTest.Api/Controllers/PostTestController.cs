using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Tiny.Http.Tests.Models;

namespace Tiny.Http.ForTest.Api.Controllers
{
    [Route("api/PostTest")]
    [ApiController]
    public class PostTestController : ControllerBase
    {
        public PostTestController()
        {
        }

        [HttpPost("FromForm")]
        public Response FromForm([FromForm] int id, [FromForm] string data)
        {
            return new Response { Id = id, ResponseData = data };
        }

        [HttpPost("NoResponse")]
        public Task NoResponse([FromBody] Request postRequest)
        {
            return Task.Delay(1);
        }

        [HttpPost("Complex")]
        public Response Complex([FromBody] Request postRequest)
        {
            return new Response() { Id = postRequest.Id, ResponseData = postRequest.Data };
        }

        [HttpPost("Stream")]
        public Stream Stream()
        {
            var body = Request.Body;
            return body;
        }
    }
}
