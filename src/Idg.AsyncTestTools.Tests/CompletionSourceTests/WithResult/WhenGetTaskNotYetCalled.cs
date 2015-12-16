using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.WithResult
{
    public class WhenGetTaskNotYetCalled : CompletionSourceWithResultTestBase
    {
        [Fact]
        public void SingleWaitForOneTaskIsNotComplete()
        {
            Assert.False(Source.WaitAsync().IsCompleted);
        }

        [Fact]
        public void SingleWaitForMultipleTaskIsNotComplete()
        {
            Assert.False(Source.WaitAsync(3).IsCompleted);
        }

        [Fact]
        public void MultipleWaitForOneTasksAreNotComplete()
        {
            Assert.False(Source.WaitAsync().IsCompleted);
            Assert.False(Source.WaitAsync().IsCompleted);
        }
    }
}
