using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Tiny.Http.Tests.Models;

namespace Tiny.Http.ForTest.Api.Controllers
{
    [Route("api/PutTest")]
    [ApiController]
    public class PutTestController : ControllerBase
    {
        public PutTestController()
        {
        }

        [HttpPut("NoResponse")]
        public Task NoResponse([FromBody] Request request)
        {
            return Task.Delay(1);
        }

        [HttpPut("Complex")]
        public Response Complex([FromBody] Request request)
        {
            return new Response() { Id = request.Id, ResponseData = request.Data };
        }
    }
}
