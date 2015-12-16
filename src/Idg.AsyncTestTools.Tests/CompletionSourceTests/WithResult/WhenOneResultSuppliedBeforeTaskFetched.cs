using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.WithResult
{
    public class WhenOneResultSuppliedBeforeTaskFetched : CompletionSourceWithResultTestBase
    {
        private Task<string>[] _tasks;

        public WhenOneResultSuppliedBeforeTaskFetched()
        {
            Source.SupplyResult(ResultOne);
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask()
            };
        }

        [Fact]
        public void FirstTaskProducesResult()
        {
            Assert.True(_tasks[0].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Equal(ResultOne, _tasks[0].Result);
        }

        [Fact]
        public void SecondTaskIsNotComplete()
        {
            Assert.False(_tasks[1].IsCompleted);
        }
    }
}
