using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.NoResult
{
    public class WhenGetTaskCalledOnceAfterDoubleWait : CompletionSourceNoResultAndArgumentTestBase
    {
        private Task _wait;

        public WhenGetTaskCalledOnceAfterDoubleWait()
        {
            _wait = Source.WaitAsync(2);
            Source.GetTask(ArgOne);
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

        [Fact]
        public void CallCountIsOne()
        {
            Assert.Equal(1, Source.CallCount);
        }
    }
}
