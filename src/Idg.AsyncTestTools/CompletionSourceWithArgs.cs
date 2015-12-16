using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Idg.AsyncTest
{
    public class CompletionSourceWithArgs<TArg>
    {
        private readonly CompletionSourceWithArgs<TArg, object> _source =
            new CompletionSourceWithArgs<TArg, object>();

        /// <summary>
        /// Returns all of the arguments passed to <see cref="GetTask(TArg)"/>.
        /// </summary>
        public IReadOnlyList<TArg> Arguments => _source.Arguments;

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
        public Task GetTask(TArg argument)
        {
            return _source.GetTask(argument);
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
    /// been invoked, and to retrieve an argument object associated with each operation.
    /// </summary>
    /// <typeparam name="TArg">
    /// The type of argument to associate with each operation.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The result type produced by each task.
    /// </typeparam>
    /// <remarks>
    /// This is very similar to <see cref="CompletionSource{TResult}"/>, adding the ability for
    /// tests to associate a single argument with each call to <see cref="GetTask(TArg)"/>.
    /// </remarks>
    public class CompletionSourceWithArgs<TArg, TResult>
    {
        private readonly object _sync = new object();
        private readonly Queue<Operation> _operations = new Queue<Operation>();
        private readonly SemaphoreSlim _waiter = new SemaphoreSlim(0);
        private List<TArg> _arguments = new List<TArg>();
        private bool _getsAreAheadOfResults;

        /// <summary>
        /// Returns all of the arguments passed to <see cref="GetTask(TArg)"/>.
        /// </summary>
        public IReadOnlyList<TArg> Arguments
        {
            get
            {
                lock (_sync)
                {
                    return _arguments.ToArray();
                }
            }
        }

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
        public Task<TResult> GetTask(TArg argument)
        {
            lock (_sync)
            {
                if (_operations.Count == 0)
                {
                    _getsAreAheadOfResults = true;
                }

                Operation op;
                if (_getsAreAheadOfResults)
                {
                    op = new Operation();
                    _operations.Enqueue(op);
                }
                else
                {
                    op = _operations.Dequeue();
                }

                _arguments.Add(argument);
                _waiter.Release();
                return op.Completion.Task;
            }
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
            lock (_sync)
            {
                if (_operations.Count == 0)
                {
                    _getsAreAheadOfResults = false;
                }

                if (_getsAreAheadOfResults)
                {
                    Operation op = _operations.Dequeue();
                    op.Completion.SetResult(result);
                }
                else
                {
                    _operations.Enqueue(new Operation(result));
                }
            }
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
            lock (_sync)
            {
                if (_operations.Count == 0)
                {
                    _getsAreAheadOfResults = false;
                }

                if (_getsAreAheadOfResults)
                {
                    Operation op = _operations.Dequeue();
                    op.Completion.SetException(error);
                }
                else
                {
                    _operations.Enqueue(new Operation(error));
                }
            }
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
            return WaitAsync(1);
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
            return Task.WhenAll(Enumerable.Range(0, count).Select(i => _waiter.WaitAsync()).ToList());
        }

        private class Operation
        {
            public Operation()
            {
                Completion = new TaskCompletionSource<TResult>();
            }

            public Operation(TResult result) : this()
            {
                Completion.SetResult(result);
            }

            public Operation(Exception error) : this()
            {
                Completion.SetException(error);
            }

            public TaskCompletionSource<TResult> Completion { get; }
        }
    }
}
