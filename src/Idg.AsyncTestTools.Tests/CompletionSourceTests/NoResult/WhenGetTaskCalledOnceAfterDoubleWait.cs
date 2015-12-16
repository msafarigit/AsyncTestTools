using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenGetTaskCalledOnceAfterDoubleWait : CompletionSourceNoResultTestBase
    {
        private Task _wait;

        public WhenGetTaskCalledOnceAfterDoubleWait()
        {
            _wait = Source.WaitAsync(2);
            Source.GetTask();
        }

        [Fact]
        public void WaitDoesNotComplete()
        {
            Assert.False(_wait.IsCompleted);
        }
    }
}
