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

        /// <summary>
        /// Produces a task that completes when the input task completes, unless the specified
        /// timeout elapses, in which case it faults with a <see cref="TimeoutException"/>.
        /// </summary>
        /// <param name="t">The task to wait for.</param>
        /// <param name="timeout">The maximum time to wait before timing out.</param>
        /// <returns>
        /// A task that represents the outcome of the input task, unless the input task did not
        /// complete within the specified time, in which case the returned task faults with
        /// <see cref="TimeoutException"/>.
        /// </returns>
        public static async Task WithTimeout(this Task t, TimeSpan timeout)
        {
            await Task.WhenAny(t, Task.Delay(timeout));
            if (!t.IsCompleted)
            {
                throw new TimeoutException();
            }
            await t;
        }

        /// <summary>
        /// Produces a task that completes when the input task completes, unless two seconds
        /// pass, in which case it faults with a <see cref="TimeoutException"/>.
        /// </summary>
        /// <param name="t">The task to wait for.</param>
        /// <returns>
        /// A task that represents the outcome of the input task, unless the input task did not
        /// complete within the specified time, in which case the returned task faults with
        /// <see cref="TimeoutException"/>.
        /// </returns>
        public static Task WithTimeout(this Task t) => t.WithTimeout(TimeSpan.FromSeconds(2));
    }
}
