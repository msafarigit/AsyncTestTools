using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.WithResult
{
    public class WhenOneExceptionSuppliedBeforeAndOneResultAfterTasksFetched : CompletionSourceWithResultTestBase
    {
        private Task<string>[] _tasks;

        public WhenOneExceptionSuppliedBeforeAndOneResultAfterTasksFetched()
        {
            Source.SupplyException(ErrorOne);
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask(),
                Source.GetTask()
            };
            Source.SupplyResult(ResultTwo);
        }

        [Fact]
        public void FirstTaskProducesException()
        {
            Assert.True(_tasks[0].ContinueWith(_ => { }).Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Same(ErrorOne, _tasks[0].Exception.InnerException);
        }

        [Fact]
        public void SecondTaskProducesResult()
        {
            Assert.True(_tasks[1].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Equal(ResultTwo, _tasks[1].Result);
        }

        [Fact]
        public void ThirdTaskIsNotComplete()
        {
            Assert.False(_tasks[2].IsCompleted);
        }
    }
}
