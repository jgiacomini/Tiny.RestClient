namespace Tiny.RestClient
{
    /// <summary>
    /// Extension to add easily debug listener
    /// </summary>
    public static class PostManListenerExtension
    {
        /// <summary>
        /// Add <see cref="PostManListener"/> to listeners />
        /// </summary>
        /// <param name="listeners">all listeners</param>
        /// <param name="name">name of the collection</param>
        /// <returns>listener created</returns>
        public static PostManListener AddPostMan(this Listeners listeners, string name)
        {
            var listener = new PostManListener(name);
            listeners.Add(listener);

            return listener;
        }
    }
}
