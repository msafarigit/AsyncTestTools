using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.WithResult
{
    public class WhenGetTaskNotYetCalled : CompletionSourceWithResultAndArgumentTestBase
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

        [Fact]
        public void CallCountIsZero()
        {
            Assert.Equal(0, Source.CallCount);
        }
    }
}
