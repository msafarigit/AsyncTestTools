using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.NoResult
{
    public class WhenGetTaskNotYetCalled : CompletionSourceNoResultAndArgumentTestBase
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
