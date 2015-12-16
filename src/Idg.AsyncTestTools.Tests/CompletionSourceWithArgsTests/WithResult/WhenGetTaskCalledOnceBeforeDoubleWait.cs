using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.WithResult
{
    public class WhenGetTaskCalledOnceBeforeDoubleWait : CompletionSourceWithResultAndArgumentTestBase
    {
        private Task _wait;

        public WhenGetTaskCalledOnceBeforeDoubleWait()
        {
            Source.GetTask(ArgOne);
            _wait = Source.WaitAsync(2);
        }


        [Fact]
        public void WaitDoesNotComplete()
        {
            Assert.False(_wait.IsCompleted);
        }

        [Fact]
        public void ReportsOneArgument()
        {
            Assert.Equal(1, Source.Arguments.Count);
        }

        [Fact]
        public void RecordsFirstArgument()
        {
            Assert.Same(ArgOne, Source.Arguments[0]);
        }
    }
}
