namespace Tiny.Http
{
    public static class DebugListernerExtension
    {
        public static void AddDebug(this Listeners listeners, bool measureTime = true)
        {
            listeners.Add(new DebugListener(measureTime));
        }
    }
}
