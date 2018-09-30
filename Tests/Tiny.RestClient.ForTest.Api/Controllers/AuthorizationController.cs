using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tiny.RestClient.ForTest.Api.Filter;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    [Route("api/Authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [HttpGet("BasicAuthentication")]
        [BasicAuthorization]
        public Task BasicAuthentication()
        {
            return Task.Delay(0);
        }

        [HttpGet("BearerAuthentication")]
        public ActionResult BearerAuthentication()
        {
            var authorization = Request.Headers["Authorization"].FirstOrDefault();
            if (authorization == null)
            {
                return Unauthorized();
            }

            return StatusCode(200);
        }
    }
}
