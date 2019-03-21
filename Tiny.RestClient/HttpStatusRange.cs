namespace Tiny.RestClient
{
    /// <summary>
    /// Represent a range of http code
    /// </summary>
    public class HttpStatusRange
    {
        /// <summary>
        /// Min http status
        /// </summary>
        public int MinHttpStatus { get; set; }

        /// <summary>
        /// MAx http status
        /// </summary>
        public int MaxHttpStatus { get; set; }
    }
}