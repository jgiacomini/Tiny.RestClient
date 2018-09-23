#if COMPLETED_TASK_NOT_SUPPORTED
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