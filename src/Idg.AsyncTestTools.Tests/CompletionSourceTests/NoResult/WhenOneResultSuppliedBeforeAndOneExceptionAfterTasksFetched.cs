using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenOneResultSuppliedBeforeAndOneExceptionAfterTasksFetched : CompletionSourceNoResultTestBase
    {
        private Task[] _tasks;

        public WhenOneResultSuppliedBeforeAndOneExceptionAfterTasksFetched()
        {
            Source.Complete();
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask(),
                Source.GetTask()
            };
            Source.SupplyException(ErrorTwo);
        }

        [Fact]
        public void FirstTaskCompletes()
        {
            Assert.True(_tasks[0].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
        }

        [Fact]
        public void SecondTaskProducesException()
        {
            Assert.True(_tasks[1].ContinueWith(_ => { }).Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Same(ErrorTwo, _tasks[1].Exception.InnerException);
        }

        [Fact]
        public void ThirdTaskIsNotComplete()
        {
            Assert.False(_tasks[2].IsCompleted);
        }
    }
}
