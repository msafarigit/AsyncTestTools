using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenGetTaskCalledOnceBeforeDoubleWait : CompletionSourceNoResultTestBase
    {
        private Task _wait;

        public WhenGetTaskCalledOnceBeforeDoubleWait()
        {
            Source.GetTask();
            _wait = Source.WaitAsync(2);
        }


        [Fact]
        public void WaitDoesNotComplete()
        {
            Assert.False(_wait.IsCompleted);
        }

        [Fact]
        public void CallCountIsOne()
        {
            Assert.Equal(1, Source.CallCount);
        }
    }
}
