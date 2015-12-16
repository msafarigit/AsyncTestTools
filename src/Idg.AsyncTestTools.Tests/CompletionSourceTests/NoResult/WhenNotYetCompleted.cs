using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenNotYetCompleted : CompletionSourceNoResultTestBase
    {
        private Task[] _tasks;

        public WhenNotYetCompleted()
        {
            _tasks = new[]
            {
                Source.GetTask(),
                Source.GetTask()
            };
        }

        [Fact]
        public void FirstTaskIsNotComplete()
        {
            Assert.False(_tasks[0].IsCompleted);
        }

        [Fact]
        public void SecondTaskIsNotComplete()
        {
            Assert.False(_tasks[1].IsCompleted);
        }
    }
}
