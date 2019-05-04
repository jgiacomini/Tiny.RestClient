using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    public class HeaderTestController : Controller
    {
        public IActionResult Get()
        {
            Response.Headers.Add("CustomHeader", "CustomValue");

            return BadRequest();
        }
    }
}