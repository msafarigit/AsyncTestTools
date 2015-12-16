using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenOneResultSuppliedBeforeTaskFetched : CompletionSourceNoResultTestBase
    {
        private Task[] _tasks;

        public WhenOneResultSuppliedBeforeTaskFetched()
        {
            Source.Complete();
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask()
            };
        }

        [Fact]
        public void FirstTaskCompletes()
        {
            Assert.True(_tasks[0].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
        }

        [Fact]
        public void SecondTaskIsNotComplete()
        {
            Assert.False(_tasks[1].IsCompleted);
        }
    }
}
