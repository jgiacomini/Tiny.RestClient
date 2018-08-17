using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Tiny.Http.Models;

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
        public Task NoResponse([FromBody] PostRequest postRequest)
        {
            return Task.Delay(1);
        }

        [HttpPut("Complex")]
        public PostResponse Complex([FromBody] PostRequest postRequest)
        {
            return new PostResponse() { Id = postRequest.Id, ResponseData = postRequest.Data };
        }

        [HttpPut("Stream")]
        public Stream Stream([FromBody] PostRequest postRequest)
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
