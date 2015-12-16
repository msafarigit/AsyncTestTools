using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenOneExceptionSuppliedBeforeTaskFetched : CompletionSourceNoResultTestBase
    {
        private Task[] _tasks;

        public WhenOneExceptionSuppliedBeforeTaskFetched()
        {
            Source.SupplyException(ErrorOne);
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask()
            };
        }

        [Fact]
        public void FirstTaskProducesException()
        {
            Assert.True(_tasks[0].ContinueWith(_ => { }).Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Same(ErrorOne, _tasks[0].Exception.InnerException);
        }

        [Fact]
        public void SecondTaskIsNotComplete()
        {
            Assert.False(_tasks[1].IsCompleted);
        }
    }
}
