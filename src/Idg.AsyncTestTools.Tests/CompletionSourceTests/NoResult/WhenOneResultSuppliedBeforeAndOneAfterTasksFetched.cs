using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenOneResultSuppliedBeforeAndOneAfterTasksFetched : CompletionSourceNoResultTestBase
    {
        private Task[] _tasks;

        public WhenOneResultSuppliedBeforeAndOneAfterTasksFetched()
        {
            Source.Complete();
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask(),
                Source.GetTask()
            };
            Source.Complete();
        }

        [Fact]
        public void FirstTaskCompletes()
        {
            Assert.True(_tasks[0].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
        }

        [Fact]
        public void SecondTaskCompletes()
        {
            Assert.True(_tasks[1].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
        }

        [Fact]
        public void ThirdTaskIsNotComplete()
        {
            Assert.False(_tasks[2].IsCompleted);
        }
    }
}
