using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    [Route("api/TimeoutTest")]
    [ApiController]
    public class TimeoutController : ControllerBase
    {
        [HttpGet("Action2Secs")]
        public async Task Action2Secs()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}
