using System.Collections.Generic;
using System.Linq;

namespace Tiny.RestClient
{
    /// <summary>
    /// Represent a collection ofstatus range
    /// </summary>
    public class HttpStatusRanges : List<HttpStatusRange>
    {
        internal HttpStatusRanges()
        {
        }

        /// <summary>
        /// Allow all status codes
        /// </summary>
        public bool AllowAllStatus { get; set; }

        /// <summary>
        /// Check if httpStatus is allowed
        /// </summary>
        /// <param name="statusCode">status code to check</param>
        /// <returns></returns>
        public bool CheckIfHttpStatusIsAllowed(int statusCode)
        {
            if (AllowAllStatus)
            {
                return true;
            }

            return this.Any(r => r.MinHttpStatus <= statusCode && r.MaxHttpStatus >= statusCode);
        }
    }
}