using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Http.ForTest.Api.Controllers
{
    [Route("api/MultiPart")]
    [ApiController]
    public class MultiPartTestController : ControllerBase
    {
        public MultiPartTestController()
        {
        }

        [HttpPost("Test")]
        public async Task<bool> Test()
        {
            var boundary = GetBoundary(Request.ContentType);
            var reader = new MultipartReader(boundary, Request.Body);

            MultipartSection section;

            while ((section = await reader.ReadNextSectionAsync()) != null)
            {
                var contentDispo = section.GetContentDispositionHeader();
                if (contentDispo.IsFileDisposition())
                {
                    var fileSection = section.AsFileSection();
                    //// await Helpers.ReadStream(fileSection.FileStream, bufferSize);
                }
                else if (contentDispo.IsFormDisposition())
                {
                    var formSection = section.AsFormDataSection();
                    var value = await formSection.GetValueAsync();
                }
            }

            return true;
        }

        private static string GetBoundary(string contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var elements = contentType.Split(' ');
            var element = elements.First(entry => entry.StartsWith("boundary="));
            var boundary = element.Substring("boundary=".Length);

            boundary = HeaderUtilities.RemoveQuotes(new Microsoft.Extensions.Primitives.StringSegment(boundary)).Value;
            return boundary;
        }
    }
}
