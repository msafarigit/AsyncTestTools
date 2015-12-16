using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.WithResult
{
    public class WhenOneResultSuppliedBeforeAndOneExceptionAfterTasksFetched : CompletionSourceWithResultTestBase
    {
        private Task<string>[] _tasks;

        public WhenOneResultSuppliedBeforeAndOneExceptionAfterTasksFetched()
        {
            Source.SupplyResult(ResultOne);
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask(),
                Source.GetTask()
            };
            Source.SupplyException(ErrorTwo);
        }

        [Fact]
        public void FirstTaskProducesResult()
        {
            Assert.True(_tasks[0].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Equal(ResultOne, _tasks[0].Result);
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
