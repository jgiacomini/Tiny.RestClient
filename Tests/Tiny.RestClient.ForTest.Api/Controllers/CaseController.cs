using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tiny.RestClient.Tests.Models;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class CaseController : Controller
    {
        // GET: api/<controller>
        [HttpGet("Camel")]
        public CamelResponse CamelCase()
        {
            return new CamelResponse() { Id = 42, ResponseData = "REP" };
        }

        [HttpGet("Snake")]
        public SnakeResponse SnakeCase()
        {
            return new SnakeResponse() { Id = 42, ResponseData = "REP" };
        }

        [HttpGet("Kebab")]
        public KebabResponse KebabCase()
        {
            return new KebabResponse() { Id = 42, ResponseData = "REP" };
        }
    }
}
