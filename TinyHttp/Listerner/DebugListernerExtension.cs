namespace Tiny.Http
{
    public static class DebugListernerExtension
    {
        public static void AddDebug(this Listeners listeners)
        {
            listeners.Add(new DebugListener());
        }
    }
}
