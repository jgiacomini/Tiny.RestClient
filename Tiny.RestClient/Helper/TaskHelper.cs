#if NETFX_45
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    internal static class TaskHelper
    {
        private static Task _completedTask;

        ///  <summary>Gets a task that's already been completed successfully.</summary>
        ///  <remarks>May not always return the same instance.</remarks>
        public static Task CompletedTask
        {
            get
            {
                var completedTask = _completedTask;

                if (completedTask == null)
                {
                    _completedTask = Task.FromResult(true);
                }

                return _completedTask;
            }
        }
    }
}
#endif