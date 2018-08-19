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
        public Task NoResponse([FromBody] Request postRequest)
        {
            return Task.Delay(1);
        }

        [HttpPut("Complex")]
        public Response Complex([FromBody] Request postRequest)
        {
            return new Response() { Id = postRequest.Id, ResponseData = postRequest.Data };
        }

        [HttpPut("Stream")]
        public Stream Stream([FromBody] Request postRequest)
        {
            byte[] byteArray = new byte[postRequest.Id];

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = i % 2 == 0 ? (byte)0 : (byte)1;
            }

            Stream stream = new MemoryStream(byteArray);

            return stream;
        }
    }
}
