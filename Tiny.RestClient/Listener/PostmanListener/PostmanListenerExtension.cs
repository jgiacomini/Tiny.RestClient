namespace Tiny.RestClient
{
    /// <summary>
    /// Extension to add easily postman listener
    /// </summary>
    public static class PostmanListenerExtension
    {
        /// <summary>
        /// Add <see cref="PostmanListener"/> to listeners />
        /// </summary>
        /// <param name="listeners">all listeners</param>
        /// <param name="name">name of the collection</param>
        /// <returns>listener created</returns>
        public static PostmanListener AddPostman(this Listeners listeners, string name)
        {
            var listener = new PostmanListener(name);
            listeners.Add(listener);

            return listener;
        }
    }
}
