using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Tiny.Http.Tests.Models;

namespace Tiny.Http.ForTest.Api.Controllers
{
    [Route("api/DeleteTest")]
    [ApiController]
    public class DeleteTestController : ControllerBase
    {
        public DeleteTestController()
        {
        }

        [HttpDelete("NoResponse")]
        public Task NoResponse()
        {
            return Task.Delay(1);
        }

        [HttpDelete("Complex")]
        public Response Complex(int id, string data)
        {
            return new Response() { Id = id, ResponseData = data };
        }

        [HttpDelete("Stream")]
        public Stream Stream()
        {
            var body = Request.Body;

            return body;
        }
    }
}
