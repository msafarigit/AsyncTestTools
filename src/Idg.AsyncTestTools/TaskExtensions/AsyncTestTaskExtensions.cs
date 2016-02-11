using System;
using System.Threading.Tasks;

namespace Idg.AsyncTest.TaskExtensions
{
    public static class AsyncTestTaskExtensions
    {
        /// <summary>
        /// Produces a task that completes without errors when the input task completes, even
        /// if the input task faults or is canceled.
        /// </summary>
        /// <param name="t">The task to wait for.</param>
        /// <returns>
        /// A task that completes when the input tasks completes, but which will always complete
        /// successfully.
        /// </returns>
        /// <remarks>
        /// This is useful if a test needs to wait for a task to complete in scenarios where
        /// exceptions are expected.
        /// </remarks>
        public static Task WhenCompleteIgnoringErrors(this Task t)
        {
            return t.ContinueWith(
                ot =>
                {
                    if (ot.IsFaulted)
                    {
                        // Observe the exception to avoid deferred reports of
                        // unhandled exceptions.
                        GC.KeepAlive(ot.Exception);
                    }
                },
                TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}
