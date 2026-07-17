#if VALUE_TASK_FROM_RESULT_NOT_SUPPORTED
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    internal static class ValueTaskHelper
    {
        /// <summary>Creates a System.Threading.Tasks.ValueTask'1 that's completed successfully with the specified result.</summary>
        /// <returns>The successfully completed task.</returns>
        public static ValueTask<T> FromResult<T>(T result)
        {
            return new ValueTask<T>(result);
        }
    }
}
#endif