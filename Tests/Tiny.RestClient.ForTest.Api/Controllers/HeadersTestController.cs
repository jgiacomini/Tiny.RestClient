using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    [Route("api/HeadersTest")]
    [ApiController]
    public class HeadersTestController : ControllerBase
    {
        [HttpGet("NoResponse")]
        public Task NoResponse()
        {
            foreach (var header in Request.Headers)
            {
                Response.Headers.Add("FROM_CLIENT" + header.Key, header.Value);
            }

            return Task.Delay(1);
        }
    }
}