using System.Diagnostics;

namespace Tiny.RestClient
{
    /// <summary>
    /// Extension to add easily debug listener.
    /// </summary>
    public static class DebugListenerExtension
    {
        /// <summary>
        /// Add <see cref="DebugListener"/> to listeners />.
        /// </summary>
        /// <param name="listeners">all listeners.</param>
        /// <param name="measureTime">allow to measure time to this listener.</param>
        [Conditional("DEBUG")]
        public static void AddDebug(this Listeners listeners, bool measureTime = true)
        {
            listeners.Add(new DebugListener(measureTime));
        }
    }
}
