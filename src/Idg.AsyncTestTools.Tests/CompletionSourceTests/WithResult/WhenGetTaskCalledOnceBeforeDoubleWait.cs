using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.WithResult
{
    public class WhenGetTaskCalledOnceBeforeDoubleWait : CompletionSourceWithResultTestBase
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
    }
}
