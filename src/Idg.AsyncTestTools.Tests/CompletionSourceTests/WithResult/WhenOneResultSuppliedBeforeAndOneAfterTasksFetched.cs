using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.WithResult
{
    public class WhenOneResultSuppliedBeforeAndOneAfterTasksFetched : CompletionSourceWithResultTestBase
    {
        private Task<string>[] _tasks;

        public WhenOneResultSuppliedBeforeAndOneAfterTasksFetched()
        {
            Source.SupplyResult(ResultOne);
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask(),
                Source.GetTask()
            };
            Source.SupplyResult(ResultTwo);
        }

        [Fact]
        public void FirstTaskProducesResult()
        {
            Assert.True(_tasks[0].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Equal(ResultOne, _tasks[0].Result);
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
