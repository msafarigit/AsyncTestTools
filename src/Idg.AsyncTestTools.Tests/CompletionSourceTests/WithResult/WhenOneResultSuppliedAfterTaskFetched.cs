using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.WithResult
{
    public class WhenOneResultSuppliedAfterTaskFetched  : CompletionSourceWithResultTestBase
    {
        private Task<string>[] _tasks;

        public WhenOneResultSuppliedAfterTaskFetched()
        {
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask()
            };
            Source.SupplyResult(ResultOne);
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
