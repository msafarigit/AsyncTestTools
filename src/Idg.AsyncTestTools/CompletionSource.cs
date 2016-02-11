using System;
using System.Threading.Tasks;

namespace Idg.AsyncTest
{
    /// <summary>
    /// Provides one or more <see cref="Task"/> objects, enabling tests to control exactly when the
    /// tasks complete, and also to discover when the operations requiring the tasks have been
    /// invoked.
    /// </summary>
    /// <remarks>
    /// This performs the same job as <see cref="CompletionSource{TResult}"/>, but is used for
    /// operations that return a plain <see cref="Task"/> that produces no result.
    /// </remarks>
    public class CompletionSource
    {
        private readonly CompletionSourceWithArgs<object, object> _source =
            new CompletionSourceWithArgs<object, object>();

        /// <summary>
        /// Returns the number of calls that have been made (as represented by calls to
        /// <see cref="GetTask"/>.
        /// </summary>
        public int CallCount => _source.CallCount;

        /// <summary>
        /// Returns a task that produces a result from a matching call to <see cref="Complete"/>,
        /// or an exception from a matching call to <see cref="SupplyException(Exception)"/>.
        /// </summary>
        /// <returns>
        /// A task that either completes immediately (which will happen if the matching call to
        /// <see cref="Complete"/> has already been made), faults immediately (which will happen
        /// if a matching call to <see cref="SupplyException(Exception)"/> has already been made)
        /// or which will complete or fault once the matching call is made.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Each call to this method produces a new task. You can have multiple tasks outstanding,
        /// with each call to <see cref="Complete"/> or <see cref="SupplyException(Exception)"/>
        /// completing one task (in the order in which they were returned by
        /// <see cref="GetTask"/>). You can also supply results in advance - if you call
        /// <see cref="Complete"/> and/or <see cref="SupplyException(Exception)"/> one or more
        /// times before <see cref="GetTask"/>, the results are stored in a queue and each call to
        /// <see cref="GetTask"/> will return a task that is already in the completed state,
        /// containing the relevant result. (Once all results have been consumed, the next call to
        /// <see cref="GetTask"/> returns a task that will not complete until a result is
        /// supplied.)
        /// </para>
        /// </remarks>
        public Task GetTask()
        {
            return _source.GetTask(null);
        }

        /// <summary>
        /// Causes a task returned by <see cref="GetTask"/> to complete successfully. You can call
        /// this either before or after the corresponding task has been retrieved.
        /// </summary>
        /// <param name="result">
        /// The result to supply for the corresponding task returned by <see cref="GetTask"/>.
        /// </param>
        public void Complete()
        {
            _source.SupplyResult(null);
        }

        /// <summary>
        /// Supplies an exception that will become the outcome of a task returned by
        /// <see cref="GetTask"/>. You can supply the exception either before or after the
        /// corresponding task has been retrieved.
        /// </summary>
        /// <param name="error">
        /// The exception to supply for the corresponding task returned by <see cref="GetTask"/>.
        /// </param>
        public void SupplyException(Exception error)
        {
            _source.SupplyException(error);
        }

        /// <summary>
        /// Waits for one call to <see cref="GetTask"/>.
        /// </summary>
        /// <returns>
        /// If the number of calls made so far to <see cref="GetTask"/> is greater than or equal
        /// to the total number of waits (calls to <see cref="WaitAsync"/> and
        /// <see cref="WaitAsync(int)"/>) this returns a task that completes immediately. Otherwise
        /// it returns a task that completes only when the number of calls to <see cref="GetTask"/>
        /// reaches the number of waits.
        /// </returns>
        public Task WaitAsync()
        {
            return _source.WaitAsync();
        }

        /// <summary>
        /// Waits for the specified number of calls to <see cref="GetTask"/>.
        /// </summary>
        /// <param name="count">
        /// The number of calls to wait for.
        /// </param>
        /// <returns>
        /// If the number of calls made so far to <see cref="GetTask"/> is greater than or equal
        /// to the total number of waits (calls to <see cref="WaitAsync"/> and
        /// <see cref="WaitAsync(int)"/>) this returns a task that completes immediately. Otherwise
        /// it returns a task that completes only when the number of calls to <see cref="GetTask"/>
        /// reaches the number of waits.
        /// </returns>
        public Task WaitAsync(int count)
        {
            return _source.WaitAsync(count);
        }
    }

    /// <summary>
    /// Provides one or more <see cref="Task{TResult}"/> objects, enabling tests to control exactly
    /// when the tasks complete, and also to discover when the operations requiring the tasks have
    /// been invoked.
    /// </summary>
    /// <typeparam name="TResult">
    /// The result type produced by each task.
    /// </typeparam>
    public class CompletionSource<TResult>
    {
        private readonly CompletionSourceWithArgs<object, TResult> _source =
            new CompletionSourceWithArgs<object, TResult>();

        /// <summary>
        /// Returns the number of calls that have been made (as represented by calls to
        /// <see cref="GetTask"/>.
        /// </summary>
        public int CallCount => _source.CallCount;

        /// <summary>
        /// Returns a task that produces a result from a matching call to
        /// <see cref="SupplyResult(TResult)"/>or an exception from a matching call to
        /// <see cref="SupplyException(Exception)"/>.
        /// </summary>
        /// <returns>
        /// A task that either completes immediately (which will happen if the matching call to
        /// <see cref="SupplyResult(TResult)"/> has already been made), faults immediately (which
        /// will happen if a matching call to <see cref="SupplyException(Exception)"/> has already
        /// been made) or which will complete or fault once the matching call is made.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Each call to this method produces a new task. You can have multiple tasks outstanding,
        /// with each call to <see cref="SupplyResult(TResult)"/> or
        /// <see cref="SupplyException(Exception)"/> completing one task (in the order in which
        /// they were returned by <see cref="GetTask"/>). You can also supply results in advance -
        /// if you call <see cref="SupplyResult(TResult)"/> and/or
        /// <see cref="SupplyException(Exception)"/> one or more times before
        /// <see cref="GetTask"/>, the results are stored in a queue and each call to
        /// <see cref="GetTask"/> will return a task that is already in the completed state,
        /// containing the relevant result. (Once all results have been consumed, the next call to
        /// <see cref="GetTask"/> returns a task that will not complete until a result is
        /// supplied.)
        /// </para>
        /// </remarks>
        public Task<TResult> GetTask()
        {
            return _source.GetTask(null);
        }

        /// <summary>
        /// Supplies a value that will become the result of a task returned by
        /// <see cref="GetTask"/>. You can supply the result either before or after the
        /// corresponding task has been retrieved.
        /// </summary>
        /// <param name="result">
        /// The result to supply for the corresponding task returned by <see cref="GetTask"/>.
        /// </param>
        public void SupplyResult(TResult result)
        {
            _source.SupplyResult(result);
        }

        /// <summary>
        /// Supplies an exception that will become the outcome of a task returned by
        /// <see cref="GetTask"/>. You can supply the exception either before or after the
        /// corresponding task has been retrieved.
        /// </summary>
        /// <param name="error">
        /// The exception to supply for the corresponding task returned by <see cref="GetTask"/>.
        /// </param>
        public void SupplyException(Exception error)
        {
            _source.SupplyException(error);
        }

        /// <summary>
        /// Waits for one call to <see cref="GetTask"/>.
        /// </summary>
        /// <returns>
        /// If the number of calls made so far to <see cref="GetTask"/> is greater than or equal
        /// to the total number of waits (calls to <see cref="WaitAsync"/> and
        /// <see cref="WaitAsync(int)"/>) this returns a task that completes immediately. Otherwise
        /// it returns a task that completes only when the number of calls to <see cref="GetTask"/>
        /// reaches the number of waits.
        /// </returns>
        public Task WaitAsync()
        {
            return _source.WaitAsync();
        }

        /// <summary>
        /// Waits for the specified number of calls to <see cref="GetTask"/>.
        /// </summary>
        /// <param name="count">
        /// The number of calls to wait for.
        /// </param>
        /// <returns>
        /// If the number of calls made so far to <see cref="GetTask"/> is greater than or equal
        /// to the total number of waits (calls to <see cref="WaitAsync"/> and
        /// <see cref="WaitAsync(int)"/>) this returns a task that completes immediately. Otherwise
        /// it returns a task that completes only when the number of calls to <see cref="GetTask"/>
        /// reaches the number of waits.
        /// </returns>
        public Task WaitAsync(int count)
        {
            return _source.WaitAsync(count);
        }
    }
}
