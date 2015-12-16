using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenGetTaskNotYetCalled : CompletionSourceNoResultTestBase
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
