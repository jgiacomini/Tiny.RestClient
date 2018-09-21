using System.Threading.Tasks;

namespace Tiny.RestClient
{
    internal class TaskHelper
    {
        private static Task _completedTask;

        ///  <summary>Gets a task that's already been completed successfully.</summary>
        ///  <remarks>May not always return the same instance.</remarks>
        public static Task CompletedTask
        {
            get
            {
#if NETFX_45
                var completedTask = _completedTask;
                if (completedTask == null)
                {
                    _completedTask = (Task)Task.FromResult(false);
                }

                return _completedTask;
#else
                return Task.CompletedTask;
#endif

            }
        }
    }
}
