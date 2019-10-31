using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Tiny.RestClient.ForTest.Api.Controllers
{
    [Route("api/GetTest")]
    [ApiController]
    public class GetTestController : ControllerBase
    {
        public GetTestController()
        {
        }

        [HttpGet("Status500Response")]
        public IActionResult Status500Response()
        {
            return StatusCode(StatusCodes.Status501NotImplemented, new string[] { "value1", "value2" });
        }

        [HttpGet("Status409Response")]
        public IActionResult Status409Response()
        {
            return StatusCode(StatusCodes.Status409Conflict, new string[] { "value1", "value2" });
        }

        [HttpGet("NoResponse")]
        public Task NoResponse()
        {
            return Task.Delay(1);
        }

        [HttpGet("ThreeSecsResponse")]
        public Task ThreeSecsResponse()
        {
            return Task.Delay(TimeSpan.FromSeconds(3));
        }

        [HttpGet("Simple")]
        public bool Simple()
        {
            return true;
        }

        [HttpGet("Complex")]
        public IEnumerable<string> Complex()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("HeadersOfResponse")]
        public void HeadersOfResponse()
        {
            Response.Headers.Add("custom1", "custom1");
            Response.Headers.Add("custom2", "custom2");
            Response.Headers.Add("custom3", "custom3");
        }

        [HttpGet("WithHeaders")]
        public List<string> WithHeaders()
        {
            var result = new List<string>();
            foreach (var header in Request.Headers)
            {
                if (header.Key.StartsWith("header"))
                {
                    result.Add($"{header.Key}_{header.Value}");
                }
            }

            return result;
        }

        [HttpGet("QueryString")]
        public string QueryString(
            string str,
            int number,
            int? numberNullable,
            uint numberUnsigned,
            uint? numberUnsignedNullable,
            bool boolean,
            bool? boolNullable,
            double doubleNumber,
            double? doubleNumberNullable,
            decimal decimalNumber,
            decimal? decimalNumberNullable,
            float floatNumber,
            float? floatNumberNullable)
        {
            return $"{str}_{number}_{numberNullable}_{numberUnsigned}_{numberUnsignedNullable}_{boolean}_{boolNullable}_{doubleNumber}_{doubleNumberNullable}_{decimalNumber}_{decimalNumberNullable}_{floatNumber}_{floatNumberNullable}";
        }

        [HttpGet("Stream")]
        public Stream Stream()
        {
            byte[] byteArray = new byte[42];

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = i % 2 == 0 ? (byte)0 : (byte)1;
            }

            Stream stream = new MemoryStream(byteArray);

            return stream;
        }
    }
}
