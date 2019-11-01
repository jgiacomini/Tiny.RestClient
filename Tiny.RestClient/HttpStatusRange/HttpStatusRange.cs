using System;
using System.Net;

namespace Tiny.RestClient
{
    /// <summary>
    /// Represent a range of http code.
    /// </summary>
    public class HttpStatusRange
    {
        /// <summary>
        /// Contruct a status range.
        /// </summary>
        /// <param name="minHttpStatus">min status range.</param>
        /// <param name="maxHttpStatus">max status range.</param>
        public HttpStatusRange(HttpStatusCode minHttpStatus, HttpStatusCode maxHttpStatus)
            : this((int)minHttpStatus, (int)maxHttpStatus)
        {
        }

        /// <summary>
         /// Contruct a status range.
         /// </summary>
         /// <param name="minHttpStatus">min status range.</param>
         /// <param name="maxHttpStatus">max status range.</param>
        public HttpStatusRange(int minHttpStatus, int maxHttpStatus)
        {
            MinHttpStatus = minHttpStatus;
            MaxHttpStatus = maxHttpStatus;
            if (maxHttpStatus < minHttpStatus)
            {
                throw new ArgumentException($"{nameof(maxHttpStatus)} must be superior or egual to {nameof(minHttpStatus)}");
            }
        }

        /// <summary>
        /// Contruct a status range.
        /// </summary>
        /// <param name="httpStatusAllowed">httpStatus allowed.</param>
        public HttpStatusRange(HttpStatusCode httpStatusAllowed)
            : this((int)httpStatusAllowed)
        {
        }

        /// <summary>
        /// Contruct a status range.
        /// </summary>
        /// <param name="httpStatusAllowed">httpStatus allowed.</param>
        public HttpStatusRange(int httpStatusAllowed)
            : this(httpStatusAllowed, httpStatusAllowed)
        {
        }

        /// <summary>
        /// Min http status.
        /// </summary>
        public int MinHttpStatus { get; private set; }

        /// <summary>
        /// MAx http status.
        /// </summary>
        public int MaxHttpStatus { get; private set; }
    }
}