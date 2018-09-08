namespace Tiny.Http
{
    /// <summary>
    /// Extension to add easily debug listener
    /// </summary>
    public static class DebugListernerExtension
    {
        /// <summary>
        /// Add <see cref="DebugListener"/> to listeners />
        /// </summary>
        /// <param name="listeners">all listeners</param>
        /// <param name="measureTime">allow to measure time to this listener</param>
        public static void AddDebug(this Listeners listeners, bool measureTime = true)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                listeners.Add(new DebugListener(measureTime));
            }
        }
    }
}
