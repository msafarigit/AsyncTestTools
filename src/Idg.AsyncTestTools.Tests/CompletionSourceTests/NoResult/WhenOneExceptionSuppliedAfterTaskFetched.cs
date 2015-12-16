using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenOneExceptionSuppliedAfterTaskFetched  : CompletionSourceNoResultTestBase
    {
        private Task[] _tasks;

        public WhenOneExceptionSuppliedAfterTaskFetched()
        {
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask()
            };
            Source.SupplyException(ErrorOne);
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
