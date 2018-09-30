namespace Tiny.RestClient
{
    /// <summary>
    /// Extension to add easily curl listener
    /// </summary>
    public static class CurlListenerExtension
    {
        /// <summary>
        /// Add <see cref="CurlListener"/> to listeners />
        /// </summary>
        /// <param name="listeners">all listeners</param>
        /// <returns>listener created</returns>
        public static CurlListener AddCurl(this Listeners listeners)
        {
            var listener = new CurlListener();
            listeners.Add(listener);

            return listener;
        }
    }
}
