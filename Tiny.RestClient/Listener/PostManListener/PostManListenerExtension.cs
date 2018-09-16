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
        public static void AddPostMan(this Listeners listeners, string name)
        {
            listeners.Add(new PostManListener(name));
        }
    }
}
